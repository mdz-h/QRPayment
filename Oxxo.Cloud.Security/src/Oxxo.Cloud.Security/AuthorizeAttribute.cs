using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Oxxo.Cloud.Security.Application.Auth.Queries;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Security.Claims;

namespace Oxxo.Cloud.Security.WebUI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var result = new ObjectResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
            try
            {
                var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
                if (allowAnonymous)
                {
                    return;
                }

                if (string.IsNullOrEmpty(context.HttpContext.Request.Headers.Authorization) || string.IsNullOrEmpty(context.HttpContext.Request.Headers.Authorization.ToString().Split(" ").Last())
                    || !context.HttpContext.User.Claims.Any())
                {
                    context.Result = result;
                    return;
                }

                string endPoint = context.HttpContext.Request.Query[GlobalConstantHelpers.PROPERTIEENDPOINT].FirstOrDefault() ?? string.Empty;
                string token = context.HttpContext.Request.Headers.Authorization.ToString().Split(" ").Last();
                string path = context.HttpContext.Request.Path.Value ?? string.Empty;

                if (path.Contains(GlobalConstantHelpers.METHODVALIDATETOKEN) && string.IsNullOrEmpty(endPoint))
                {
                    context.Result = result;
                    return;
                }

                if (!path.Contains(GlobalConstantHelpers.METHODVALIDATETOKEN))
                {
                    AuthResponse resuAuth = ValidateToken(context, token);
                    if (string.IsNullOrWhiteSpace(resuAuth.IdGuid))
                    {
                        context.Result = result;
                        return;
                    }
                    var userIdentity = new ClaimsIdentity(new List<Claim>() 
                    {                        
                        new Claim(GlobalConstantHelpers.USERIDENTIFICATION, resuAuth.Name ?? string.Empty),
                        new Claim(GlobalConstantHelpers.IDENTIFICATION, resuAuth.IdGuid)
                    });
                    context.HttpContext.User = new ClaimsPrincipal(userIdentity);                    
                }
            }
            catch (Exception ex)
            {
                context.Result = GetHttpStatusCodeException(ex.Message);
            }
        }

        private static AuthResponse ValidateToken(AuthorizationFilterContext context, string token)
        {
            if (context.HttpContext.Request.Path.Value == null)
            {
                throw new CustomException();
            }
            string pathContext = context.HttpContext.Request.Path.Value.ToString();
            string endPoint = $"{pathContext.Split("/").Reverse().ToArray()[1]}/{pathContext.Split("/").Last()}";
            var iAuthenticateQuery = context.HttpContext.RequestServices.GetService(typeof(IAuthenticateQuery)) as IAuthenticateQuery;
            var iLogService = context.HttpContext.RequestServices.GetService(typeof(ILogService)) as ILogService;
            var iTokenGenerator = context.HttpContext.RequestServices.GetService(typeof(ITokenGenerator)) as ITokenGenerator;
            var validateToken = new ValidateTokenQueryHandler(iAuthenticateQuery, iLogService, iTokenGenerator);
            ValidateTokenQuery query = new() { BearerToken = token, Endpoint = endPoint };
            return validateToken.Handle(query, new CancellationToken()).Result;           
        }

        private static ObjectResult GetHttpStatusCodeException(string errorMessage)
        {
            if (errorMessage.Contains(GlobalConstantMessages.ERRORTOKENEXPIRED))
            {
                return new ObjectResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            if (errorMessage.Contains(GlobalConstantMessages.PERMISSIONTOKEN))
            {
                return new ObjectResult(string.Empty) { StatusCode = StatusCodes.Status403Forbidden };
            }
            return new ObjectResult(string.Empty) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}

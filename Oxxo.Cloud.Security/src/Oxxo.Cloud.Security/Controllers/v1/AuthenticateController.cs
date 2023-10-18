#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Controller authenticate.
//===============================================================================
#endregion

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate;
using Oxxo.Cloud.Security.Application.Auth.Commands.Refresh;
using Oxxo.Cloud.Security.Application.Auth.Queries;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Application.InvFis.Command.AuthInvfis;
using Oxxo.Cloud.Security.Domain.Consts;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Oxxo.Cloud.Security.WebUI.Controllers.v1;
[ApiVersion("1")]
[SwaggerTag(GlobalConstantSwagger.TAGAUTHENTICATE)]
public class AuthenticateController : ApiControllerBase
{
    private new IMediator Mediator { get; set; }
    private readonly ILogService logService;
    public AuthenticateController(IMediator mediator, ILogService logService)
    {
        Mediator = mediator;
        this.logService = logService;
    }

    /// <summary>
    /// Its executes the handler responsible for authentication for all device types.
    /// </summary>
    /// <param name="authenticateCommand">Contains the main values for authentication</param>
    /// <returns>Contains the generated token data.</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route(GlobalConstantHelpers.METHODLOGIN)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = GlobalConstantSwagger.AUTHENTICATELOGIN, Description = GlobalConstantSwagger.DESCAUTHENTICATELOGIN)]

    public async Task<IActionResult> Login([FromBody] AuthenticateCommand authenticateCommand)
    {
        try
        {
            authenticateCommand.ApiKey = Request.Headers[GlobalConstantHelpers.APIKEY];
            authenticateCommand.DeviceKey = Request.Headers[GlobalConstantHelpers.DEVICEKEY];
            authenticateCommand.Identification = Environment.GetEnvironmentVariable(GlobalConstantHelpers.URLOPERATORIDDEFAULT) ?? string.Empty;
            return Ok(await Mediator.Send(authenticateCommand));
        }
        catch (ValidationException ex)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODLOGIN, LogTypeEnum.Error,
                string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATEAPI, ex.GetException(), ex.ErrorsMessage, Request.Path.Value), GlobalConstantHelpers.NAMEAUTHENTICATECONTROLLER);
            throw;
        }
        catch (Exception ex)
        {            
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODLOGIN, LogTypeEnum.Error, 
                string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATEAPI, ex.GetException(), Request.Path.Value), GlobalConstantHelpers.NAMEAUTHENTICATECONTROLLER);
            throw;
        }
    }

    /// <summary>
    /// Its executes the handler responsible for the token update.
    /// </summary>
    /// <returns>Contains the generated token data.</returns>
    [HttpPost]
    [Route(GlobalConstantHelpers.METHODREFRESHTOKEN)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = GlobalConstantSwagger.AUTHENTICATEREFRESH, Description = GlobalConstantSwagger.DESCAUTHENTICATEREFRESH)]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {           
            return Ok(await Mediator.Send(
            new RefreshTokenCommand
            {        
                BearerToken = Request.Headers[HeaderNames.Authorization],
                RefreshToken = Request.Headers[GlobalConstantHelpers.REFRESHTOKEN],
                Identification = Environment.GetEnvironmentVariable(GlobalConstantHelpers.URLOPERATORIDDEFAULT) ?? string.Empty
            }));
        }
        catch (ValidationException ex)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODREFRESHTOKEN, GlobalConstantHelpers.METHODREFRESHTOKEN, LogTypeEnum.Error,
                string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATEAPI, ex.GetException(), ex.ErrorsMessage, Request.Path.Value), GlobalConstantHelpers.NAMEAUTHENTICATECONTROLLER);
            throw;
        }
        catch (Exception ex)
        {            
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODREFRESHTOKEN, GlobalConstantHelpers.METHODREFRESHTOKEN, LogTypeEnum.Error, 
                string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATEAPI, ex.GetException(), Request.Path.Value), GlobalConstantHelpers.NAMEAUTHENTICATECONTROLLER);
            throw;
        }
    }

    /// <summary>
    /// Its executes the handler responsible for the token validate.
    /// </summary>
    /// <returns>Contains the boolean data of the validated token.</returns>
    [Authorize]
    [HttpGet]
    [Route(GlobalConstantHelpers.METHODVALIDATETOKEN)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = GlobalConstantSwagger.AUTHENTICATEVALIDATE, Description = GlobalConstantSwagger.DESCAUTHENTICATEVALIDATE)]
    public async Task<IActionResult> ValidateToken([FromQuery] string Endpoint)
    {
        try
        {
            return Ok(await Mediator.Send(new ValidateTokenQuery { BearerToken = Request.Headers[HeaderNames.Authorization], Endpoint = Endpoint }));
        }
        catch (ValidationException ex)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODVALIDATETOKEN, GlobalConstantHelpers.METHODVALIDATETOKEN, LogTypeEnum.Error,
                string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATEAPI, ex.GetException(), ex.ErrorsMessage, Request.Path.Value), GlobalConstantHelpers.NAMEAUTHENTICATECONTROLLER);
            throw;
        }
        catch (Exception ex)
        {            
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODVALIDATETOKEN, GlobalConstantHelpers.METHODVALIDATETOKEN, LogTypeEnum.Error,
                string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATEAPI, ex.GetException(), Request.Path.Value), GlobalConstantHelpers.NAMEAUTHENTICATECONTROLLER);
            throw;
        }

    }

    [Authorize]
    [HttpPost]
    [Route(GlobalConstantHelpers.METHOD_LOGIN_INVFIS)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = GlobalConstantSwagger.AUTHENTICATELOGININVFIS, Description = GlobalConstantSwagger.DESCAUTHENTICATELOGININVFIS)]
    public async Task<IActionResult> LoginInvFis([FromBody] AuthInvfisCommand invfisQuery)
    {
        string userIdentification = string.Empty;
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;

            string authorizationHeader = Request.Headers[HeaderNames.Authorization];
            if (authorizationHeader != null && authorizationHeader.StartsWith(GlobalConstantHelpers.BEARER_WORD))
            {
                string bearerToken = authorizationHeader.Substring(GlobalConstantHelpers.BEARER_WORD.Length +1);
                invfisQuery.TokenAuth = bearerToken;
                var response = await Mediator.Send(invfisQuery);

                return StatusCode((int)response.StatusCode, response.Body);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, null);
            }           

        }
        catch (ValidationException ex)
        {
            await logService.Logger(GlobalConstantSwagger.AUTHENTICATELOGININVFIS,
                GlobalConstantHelpers.METHOD_LOGIN_INVFIS,
                LogTypeEnum.Error,
                userIdentification,
                string.Concat(GlobalConstantMessages.LOGIN_INVFIS_ERROR_API_POST, string.Concat(ex.GetException(), ex.Errors), Request.Path.Value),
                GlobalConstantSwagger.LOGIN_INVFIS_CONTROLLER);
            return StatusCode((int)HttpStatusCode.UnprocessableEntity, null);
        }
        catch (InvalidOperationException ex)
        {
            await logService.Logger(GlobalConstantSwagger.AUTHENTICATELOGININVFIS,
                GlobalConstantHelpers.METHOD_LOGIN_INVFIS,
                LogTypeEnum.Error,
                userIdentification,
                string.Concat(GlobalConstantMessages.LOGIN_INVFIS_ERROR_API_POST, ex.GetException(), Request.Path.Value),
                GlobalConstantSwagger.LOGIN_INVFIS_CONTROLLER);
            return StatusCode((int)HttpStatusCode.InternalServerError, null);
        }
        catch (Exception ex)
        {
            await logService.Logger(GlobalConstantSwagger.AUTHENTICATELOGININVFIS,
                GlobalConstantHelpers.METHOD_LOGIN_INVFIS,
                LogTypeEnum.Error,
                userIdentification,
                string.Concat(GlobalConstantMessages.LOGIN_INVFIS_ERROR_API_POST, ex.GetException(), Request.Path.Value),
                GlobalConstantSwagger.LOGIN_INVFIS_CONTROLLER);
            return StatusCode((int)HttpStatusCode.InternalServerError, null);
        }

    }


}

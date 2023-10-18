#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Command refresh token.
//===============================================================================
#endregion
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Refresh;
public class RefreshTokenCommand : BasePropertiesDto, IRequest<AuthResponse>
{
    public string? RefreshToken { get; set; }
}

public class TokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly ITokenGenerator tokenGenerator;
    private readonly IAuthenticateQuery authenticateQuery;
    private readonly IApplicationDbContext context;
    private readonly ILogService logService;

    public TokenCommandHandler(IApplicationDbContext context, ITokenGenerator tokenGenerator, IAuthenticateQuery authenticateQuery, ILogService logService)
    {
        this.context = context;
        this.tokenGenerator = tokenGenerator;
        this.authenticateQuery = authenticateQuery;
        this.logService = logService;
    }

    /// <summary>
    /// Contains all the necessary business rules to refresh a token using its old token and refresh.
    /// First, it validates that there is a session, get the token the data base and compare with send token. 
    /// After valida if token is device, external o user.Obtain the main information, place, store, name application...
    /// and we call the function  that generates the token and register the session in the database. 
    /// </summary>
    /// <param name="request">>The old token and refresh token .</param>
    /// <param name="cancellationToken"></param>
    /// <returns>AuthResponse object with the new token and refresh token.</returns>
    /// <exception cref="CustomException">Contains the custom error.</exception>
    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userSession = await context.SESSION_TOKEN.Where(x => x.TOKEN == request.Token).FirstAsync(cancellationToken: cancellationToken);
            var jwtToken = await GetTokenDeviceExternalUser(userSession.GUID, request.UserIdentification, request.Token, cancellationToken);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var newRefreshToken = tokenGenerator.GenerateRefreshToken();

            userSession.TOKEN = token;
            userSession.REFRESH_TOKEN = newRefreshToken;
            userSession.EXPIRATION_TOKEN = jwtToken.ValidTo;
            userSession.LCOUNT += 1;
            userSession.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
            userSession.MODIFIED_DATETIME = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);           
            return new AuthResponse
            {
                Auth = true,
                Token = token,
                RefreshToken = newRefreshToken,
                Exp = jwtToken.ValidTo
            };
        }
        catch (Exception ex)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODREFRESHTOKEN, GlobalConstantHelpers.METHODREFRESHTOKENHANDLER, LogTypeEnum.Error, request.UserIdentification,
                string.Concat(GlobalConstantMessages.LOGERRORREFRESHTOKENCOMMAND, ex.GetException()), GlobalConstantHelpers.NAMECLASSREFRESHTOKENCOMMANDHANDLER);
            throw new CustomException(ex.GetException());
        }
    }

    private async Task<JwtSecurityToken> GetTokenDeviceExternalUser(Guid guidId, string userIdentification, string token, CancellationToken cancellationToken)
    {
       
        var externalAplicacion = await authenticateQuery.GetExternalApplication(guidId, cancellationToken);

        if (externalAplicacion != null)
        {
            return tokenGenerator.GenerateJWTToken(externalAplicacion.NAME, externalAplicacion.CODE, externalAplicacion.TIME_EXPIRATION_TOKEN.ToString(), externalAplicacion.EXTERNAL_APPLICATION_ID.ToString());

        }
        DeviceTokenDto? deviceDto = await authenticateQuery.GetDeviceToken(guidId);
        if (deviceDto != null)
        {
            return tokenGenerator.GenerateJWTToken(deviceDto);
        }

        Operator? @operator = await authenticateQuery.GetOperatorActiveForGuidId(guidId, cancellationToken);
        if(@operator != null)
        {
            SystemParam? systemParam = await authenticateQuery.GetTimeExpirationTokenUser(cancellationToken);
            if (systemParam == null || systemParam.PARAM_VALUE == null)
            {
                throw new CustomException(GlobalConstantMessages.USERTOKENTIMEEXPIRATIONNOTFOUND, HttpStatusCode.UnprocessableEntity);
            }

            var userClaims = tokenGenerator.GetUserClaims(token);

            UserDto authResponse = new()
            {
                CrPlace = userClaims.CrPlace,
                CrStore = userClaims.CrStore,
                FullName = string.IsNullOrWhiteSpace(@operator.PERSON.MIDDLE_NAME) 
                ? $"{@operator.PERSON.NAME}|{@operator.PERSON.LASTNAME_PAT}|{@operator.PERSON.LASTNAME_MAT}" 
                : $"{@operator.PERSON.NAME}|{@operator.PERSON.MIDDLE_NAME}|{@operator.PERSON.LASTNAME_PAT}|{@operator.PERSON.LASTNAME_MAT}",
                Till = userClaims.Till
            };            
            return tokenGenerator.GenerateJWTToken(@operator.PERSON_ID, @operator.OPERATOR_ID, @operator.USER_NAME, int.Parse(systemParam.PARAM_VALUE), authResponse.CrPlace, authResponse.CrStore, authResponse.Till, authResponse.FullName);
        }

        await logService.Logger(GlobalConstantHelpers.EVENTMETHODREFRESHTOKEN, GlobalConstantHelpers.METHODREFRESHTOKENHANDLER, LogTypeEnum.Error, userIdentification,
            GlobalConstantMessages.JWTDEVICEAPPUSERNOTFOUND, GlobalConstantHelpers.NAMECLASSREFRESHTOKENCOMMANDHANDLER);
        throw new CustomException(GlobalConstantMessages.JWTDEVICEAPPUSERNOTFOUND, HttpStatusCode.UnprocessableEntity);

    }
}
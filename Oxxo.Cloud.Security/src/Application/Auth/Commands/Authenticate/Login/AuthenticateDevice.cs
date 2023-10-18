#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Authenticate device.
//===============================================================================
#endregion
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Auth.Commands.Refresh;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Login;

public class AuthenticateDevice : IAuthenticate
{
    private readonly IApplicationDbContext context;
    private readonly ITokenGenerator tokenGenerator;
    private readonly IAuthenticateQuery authenticateQuery;
    private readonly ILogService logService;
    readonly AuthenticateDto authenticate;

    /// <summary>
    /// Constructor that injects the interface token, database context and tokens querys.
    /// </summary>
    /// <param name="context">Context database</param>
    /// <param name="tokenGenerator">Inject the interface with the necessary methods to generate the token</param>
    /// <param name="authenticateQuery">Inject the interface with the necessary methods to obtener consults in the database</param>
    /// <param name="authenticate">Contains the main values for device authentication.</param>
    public AuthenticateDevice(IApplicationDbContext context, ITokenGenerator tokenGenerator, IAuthenticateQuery authenticateQuery, ILogService logService, DeviceDto authenticate)
    {
        this.context = context;
        this.tokenGenerator = tokenGenerator;
        this.authenticateQuery = authenticateQuery;
        this.logService = logService;
        this.authenticate = authenticate;
    }

    /// <summary>
    /// Contains all the necessary business rules to authenticate a device using name device (id).
    /// We obtain the main information, place, store, etc. and we call the function  that generates the token 
    /// and register the session in the database.  
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>AuthResponse object with the new token and refresh token</returns>
    public async Task<AuthResponse> Auth(CancellationToken cancellationToken)
    {
        try
        {
            if (authenticate == null)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEDEVICE, LogTypeEnum.Error, string.Concat(GlobalConstantMessages.TYPEAUTHNOTFOUND, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE), GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE);
                throw new CustomException(GlobalConstantMessages.TYPEAUTHNOTFOUND, HttpStatusCode.UnprocessableEntity);
            }

            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEDEVICE, LogTypeEnum.Information, GlobalConstantMessages.LOGINITAUTHENTICATEDEVICE, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE);

            var device = (DeviceDto)authenticate;
            var auhtResponse = await ValidateProcessExistAndRegresToken(device.Id, authenticate.Identification, cancellationToken);
            if (auhtResponse != null)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEDEVICE, LogTypeEnum.Information, GlobalConstantMessages.LOGENDTAUTHENTICATEDEVICE, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE);
                return auhtResponse;
            }

            DeviceTokenDto? deviceToken = await authenticateQuery.GetDeviceToken(device.Id);
            if (deviceToken == null)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEDEVICE, LogTypeEnum.Error, GlobalConstantMessages.VALIDATEDATATOKEN, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE);
                throw new CustomException(GlobalConstantMessages.VALIDATEDATATOKEN, HttpStatusCode.UnprocessableEntity);
            }
            deviceToken.Key = device.DeviceKey;

            AuthResponse authResponse = await RegisterToken(deviceToken, device.Identification, cancellationToken);
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEDEVICE, LogTypeEnum.Information, GlobalConstantMessages.LOGENDTAUTHENTICATEDEVICE, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE);
            return authResponse;
        }
        catch (Exception ex)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEDEVICE, LogTypeEnum.Error,
                string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATEDEVICE, ex.GetException()), GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE);
            throw;
        }
    }

    /// <summary>
    /// Validates if there is an active session, then checks the validity of the token, if it expires it calls the method that contains the business rules to update the token.
    /// </summary>
    /// <param name="nameDevice">Contains the name value for device authentication.</param>
    /// <param name="cancellationToken"></param>
    /// <param name="source">Contains the source value for log.</param>
    /// <returns></returns>
    private async Task<AuthResponse?> ValidateProcessExistAndRegresToken(string nameDevice, string identification, CancellationToken cancellationToken)
    {
        if (await authenticateQuery.ValidateSessionActiveDevice(nameDevice))
        {
            var auhtResponse = await authenticateQuery.GetSessionTokenDevice(nameDevice);
            if (auhtResponse.Exp != null && await authenticateQuery.ValidateExpirationToken(auhtResponse.Token, cancellationToken))
            {
                return auhtResponse;
            }
            else
            {
                return await UpdateToken(auhtResponse, identification, cancellationToken);
            }
        }
        return null;
    }

    /// <summary>
    /// Gets the values sent by parameter generating an object of type tokenCommandHandler and calls the event that updates the token.
    /// </summary>
    /// <param name="auhtResponse">Contains the object with data token</param>
    /// <param name="cancellationToken"></param>
    /// <param name="source">Contains the source value for log.</param>
    /// <returns></returns>
    /// <exception cref="CustomException"></exception>
    private async Task<AuthResponse> UpdateToken(AuthResponse auhtResponse, string identification, CancellationToken cancellationToken)
    {
        try
        {            
            RefreshTokenCommand command = new() { BearerToken = auhtResponse.Token, RefreshToken = auhtResponse.RefreshToken, Identification = identification };
            return await new TokenCommandHandler(context, tokenGenerator, authenticateQuery, logService).Handle(command, cancellationToken);
        }
        catch (Exception ex)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEDEVICE, LogTypeEnum.Error,
                string.Concat(GlobalConstantMessages.LOGERRORREFRESHTOKENCOMMAND, ex.GetException()), GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE);
            throw;
        }
    }

    /// <summary>
    /// Contains all the business rules needed to create a token and register it in the database.
    /// </summary>
    /// <param name="deviceToken">Object device with values for create token</param>
    /// <param name="identification">String value with identifaction guid defualt</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<AuthResponse> RegisterToken(DeviceTokenDto deviceToken, string identification, CancellationToken cancellationToken)
    {
        JwtSecurityToken jwtToken = tokenGenerator.GenerateJWTToken(deviceToken);
        string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        string newRefreshToken = tokenGenerator.GenerateRefreshToken();

        var entity = new SessionToken()
        {
            GUID = deviceToken.DeviceId,
            SESSION_STATUS_ID = GlobalConstantHelpers.SESSIONSTATUSID,
            TOKEN = token,
            REFRESH_TOKEN = newRefreshToken,
            EXPIRATION_TOKEN = jwtToken.ValidTo,
            START_DATETIME = DateTime.UtcNow,
            END_DATETIME = DateTime.MinValue,
            IS_ACTIVE = true,
            LCOUNT = 0,
            CREATED_BY_OPERATOR_ID = Guid.Parse(identification),
            CREATED_DATETIME = DateTime.UtcNow
        };
        await context.SESSION_TOKEN.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            Auth = true,
            Token = token,
            RefreshToken = newRefreshToken,
            Exp = jwtToken.ValidTo
        };
    }

}

#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Authenticate external device.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.LoginExternal;

public class AuthenticateExternal : IAuthenticate
{
    private readonly ITokenGenerator tokenGenerator;
    private readonly IApplicationDbContext context;
    private readonly ILogService logService;
    readonly AuthenticateDto authenticate;


    /// <summary>
    /// Constructor that injects the interface token and database context.
    /// </summary>
    /// <param name="context">database context</param>
    /// <param name="tokenGenerator">token</param>
    /// <param name="authenticate">Contains the main values for external authentication.</param>
    public AuthenticateExternal(IApplicationDbContext context, ITokenGenerator tokenGenerator, ILogService logService, ExternalDto authenticate)
    {
        this.context = context;
        this.tokenGenerator = tokenGenerator;
        this.logService = logService;
        this.authenticate = authenticate;
    }

    /// <summary>
    /// Contains all the necessary business rules to authenticate a external device using its code(id).
    /// With application code it obtain the main information timeExpiration, name and we call the function  that generates the token 
    /// and register the session in the database.   
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>AuthResponse object with the new token and refresh token</returns>
    /// <exception cref="CustomException">Contains the custom error.</exception>
    public async Task<AuthResponse> Auth(CancellationToken cancellationToken)
    {
        try
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEEXTERNAL, LogTypeEnum.Information, GlobalConstantMessages.LOGINITAUTHENTICATEEXTERNAL, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEEXTERNAL);
            ExternalDto external = (ExternalDto)authenticate;
            ExternalApplication externalAplicacion = await context.EXTERNAL_APPLICATION.AsNoTracking().Where(x => x.CODE == external.Id).FirstAsync(cancellationToken: cancellationToken);
            JwtSecurityToken jwtToken = tokenGenerator.GenerateJWTToken(externalAplicacion.NAME, externalAplicacion.CODE, externalAplicacion.TIME_EXPIRATION_TOKEN.ToString(), externalAplicacion.EXTERNAL_APPLICATION_ID.ToString());
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            string newRefreshToken = tokenGenerator.GenerateRefreshToken();

            var entity = new SessionToken()
            {
                GUID = externalAplicacion.EXTERNAL_APPLICATION_ID,
                SESSION_STATUS_ID = GlobalConstantHelpers.SESSIONSTATUSID,
                TOKEN = token,
                REFRESH_TOKEN = newRefreshToken,
                EXPIRATION_TOKEN = jwtToken.ValidTo,
                START_DATETIME = DateTime.UtcNow,
                END_DATETIME = DateTime.MinValue,
                IS_ACTIVE = true,
                LCOUNT = 0,
                CREATED_BY_OPERATOR_ID = Guid.Parse(external.Identification),
                CREATED_DATETIME = DateTime.UtcNow
            };
            await context.SESSION_TOKEN.AddAsync(entity, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEEXTERNAL, LogTypeEnum.Information, GlobalConstantMessages.LOGENDTAUTHENTICATEEXTERNAL, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEEXTERNAL);
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
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEEXTERNAL, LogTypeEnum.Error,
                string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATEEXTERNAL, ex.GetException(),
                GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEEXTERNAL), GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEEXTERNAL);
            throw;
        }
    }
}

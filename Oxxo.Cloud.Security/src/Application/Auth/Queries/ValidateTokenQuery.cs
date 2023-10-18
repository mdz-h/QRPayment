#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Query token validate.
//===============================================================================
#endregion
using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Auth.Queries;
public class ValidateTokenQuery : BasePropertiesDto, IRequest<AuthResponse>
{
    public string? Endpoint { get; set; }
}

public class ValidateTokenQueryHandler : IRequestHandler<ValidateTokenQuery, AuthResponse>
{
    private readonly IAuthenticateQuery authenticateQuery;
    private readonly ILogService logService;
    private readonly ITokenGenerator tokenGenerator;
    /// <summary>
    /// Constructor that injects the tokens querys.
    /// </summary>    
    /// <param name="authenticateQuery">Inject the interface with the necessary methods to obtener consults in the database</param>
    public ValidateTokenQueryHandler(IAuthenticateQuery authenticateQuery, ILogService logService, ITokenGenerator tokenGenerator)
    {
        this.authenticateQuery = authenticateQuery;
        this.logService = logService;
        this.tokenGenerator = tokenGenerator;
    }

    /// <summary>
    /// Generates the AuthResponse with bolean value in true
    /// </summary>
    /// <param name="request">Contains the object token</param>
    /// <param name="cancellationToken"></param>
    /// <returns>AuthResponse object with bolean value</returns>
    public async Task<AuthResponse> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODVALIDATETOKEN, GlobalConstantHelpers.METHODVALIDATETOKENHANDLER, LogTypeEnum.Information, string.Concat(GlobalConstantMessages.LOGINITVALIDATETOKENCOMMAND, request.Source), GlobalConstantHelpers.NAMECLASSVALIDATETOKENQUERY);
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODVALIDATETOKEN, GlobalConstantHelpers.METHODVALIDATETOKENHANDLER, LogTypeEnum.Information, $"{GlobalConstantHelpers.PROPERTIEENDPOINT}:{request.Endpoint}", GlobalConstantHelpers.NAMECLASSVALIDATETOKENQUERY);
            if (!await authenticateQuery.ValidateExpirationToken(request.Token))
            {
                throw new CustomException(GlobalConstantMessages.ERRORTOKENEXPIRED, HttpStatusCode.Unauthorized);
            }

            if (!await authenticateQuery.ValidatePermissionToken(request.Token, request.Endpoint ?? string.Empty))
            {
                throw new CustomException(GlobalConstantMessages.PERMISSIONTOKEN, HttpStatusCode.Forbidden);
            }

            var authResponse = tokenGenerator.GetNameIdClaimsToken(request.Token ?? string.Empty);

            if (string.IsNullOrEmpty(authResponse.Name) || string.IsNullOrEmpty(authResponse.IdGuid))
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODVALIDATETOKEN, GlobalConstantHelpers.METHODVALIDATETOKENHANDLER, LogTypeEnum.Error, string.Concat(GlobalConstantMessages.LOGERRORVALIDATETOKENNAMECLAIMS, request.Source), GlobalConstantHelpers.NAMECLASSVALIDATETOKENQUERY);
                throw new CustomException(GlobalConstantMessages.LOGERRORVALIDATETOKENNAMECLAIMS, HttpStatusCode.Unauthorized);
            }
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODVALIDATETOKEN, GlobalConstantHelpers.METHODVALIDATETOKENHANDLER, LogTypeEnum.Information, string.Concat(GlobalConstantMessages.LOGENDVALIDATETOKENCOMMAND, request.Source), GlobalConstantHelpers.NAMECLASSVALIDATETOKENQUERY);
            return authResponse;
        }
        catch (Exception ex)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODVALIDATETOKEN, GlobalConstantHelpers.METHODVALIDATETOKENHANDLER, LogTypeEnum.Error, string.Concat(GlobalConstantMessages.LOGERRORVALIDATETOKENCOMMAND, ex.Message, request.Source), GlobalConstantHelpers.NAMECLASSVALIDATETOKENQUERY);
            throw;
        }
    }
}

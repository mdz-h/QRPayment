#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Command authenticate.
//===============================================================================
#endregion
using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate;
public class AuthenticateCommand : BasePropertiesDto, IRequest<AuthResponse>
{

    public AuthenticateCommand()
    {
        Id = string.Empty;
        ApiKey = string.Empty;
        DeviceKey = string.Empty;
        TypeAuth = string.Empty;
        CrPlace = string.Empty;
        CrStore = string.Empty;
        User = string.Empty;
        Password = string.Empty;
        Till = string.Empty;
        AdministrativeDate = string.Empty;
        ProcessDate = string.Empty;
        Type = string.Empty;
        IsInternal = true;
        WorkGroup = string.Empty;
        StatusOperator = string.Empty;
        FullName = string.Empty;
    }

    public string Id { get; set; }
    public string ApiKey { get; set; }
    public string DeviceKey { get; set; }
    public string TypeAuth { get; set; }
    public string CrPlace { get; set; }
    public string CrStore { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Till { get; set; }
    public string AdministrativeDate { get; set; }   
    public string ProcessDate { get; set; }
    public string Type { get; set; }
    public bool IsInternal { get; set; }
    public string WorkGroup { get; set; }
    public string StatusOperator { get; set; }
    public string FullName { get; set; }
}

public class AuthenticateHandler : IRequestHandler<AuthenticateCommand, AuthResponse>
{
    private readonly ILogService logService;
    private readonly IApplicationDbContext context;
    private readonly ITokenGenerator tokenGenerator;
    private readonly IAuthenticateQuery authenticateQuery;
    private readonly ICryptographyService cryptographyService;
    private readonly ISecurity security;
    /// <summary>
    /// Constructor that injects the authenticate factory.
    /// </summary>
    /// <param name="logService"></param>
    /// <param name="context"></param>
    /// <param name="tokenGenerator"></param>
    /// <param name="authenticateQuery"></param>
    public AuthenticateHandler(ILogService logService, IApplicationDbContext context, ITokenGenerator tokenGenerator, IAuthenticateQuery authenticateQuery, ICryptographyService cryptographyService, ISecurity security)
    {
        this.logService = logService;
        this.context = context;
        this.tokenGenerator = tokenGenerator;
        this.authenticateQuery = authenticateQuery;
        this.cryptographyService = cryptographyService;
        this.security = security;
    }

    /// <summary>
    /// Contains all the business rules necessary to obtain the type de authentication, device, external o user.
    /// Implements the abstract factory design pattern 
    /// </summary>
    /// <param name="request">Parameters request</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Type of Authenticate</returns>
    public async Task<AuthResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var authenticate = request.GetAuthenticateType(context, tokenGenerator, authenticateQuery, logService, cryptographyService, security);
            return await authenticate.Auth(cancellationToken);
        }
        catch (Exception ex)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEHANDLER, LogTypeEnum.Error, string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATECOMMAND, ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex.StackTrace, request.Source), GlobalConstantHelpers.NAMECLASSAUTHENTICATECOMMANDHANDLER);
            throw;
        }
    }
}

#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-21.
// Comment: GENERATE API KEY.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Extensions;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.GenerateApiKey
{
    /// <summary>
    /// Class DTO to transfer information to generate API Keys
    /// </summary>
    public class GenerateApiKeyCommand : BasePropertiesDto, IRequest<ApiKeyDto>
    {
        public GenerateApiKeyCommand() { ExternalAppId = string.Empty; }
        public string ExternalAppId { get; set; }
    }

    /// <summary>
    /// Principal process to generate an API Key
    /// </summary>
    public class GenerateApiKeyHandler : IRequestHandler<GenerateApiKeyCommand, ApiKeyDto>
    {
        private readonly ILogService logService;
        private readonly IApplicationDbContext context;
        private readonly ISecurity security;
        private readonly ICryptographyService cryptography;

        /// <summary>
        /// Constructor that injects the interface ILogService and ICreateAdministrator.
        /// </summary>            
        /// <param name="logService">Inject the interface necessary for save the log</param>
        /// <param name="context">Inject the interface necessary for create administrators</param>
        /// <param name="security">Inject Security Process</param>
        public GenerateApiKeyHandler(ILogService logService, IApplicationDbContext context, ISecurity security, ICryptographyService cryptography)
        {
            this.logService = logService;
            this.context = context;
            this.security = security;
            this.cryptography = cryptography;
        }

        /// <summary>
        /// This method generate an API Key to relations an external application
        /// </summary>
        /// <param name="request">parameters</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>API Key</returns>
        public async Task<ApiKeyDto> Handle(GenerateApiKeyCommand request, CancellationToken cancellationToken)
        {
            ApiKeyDto apiKeydto = new();

            try
            {
                List<SystemParam> lstSystemParams = await security.GetSystemParamsApiKeyRules(cancellationToken);
                string apiKeygenerated = await GetApiKey(lstSystemParams, cancellationToken);
                int iDays = lstSystemParams.Where(w => w.PARAM_CODE == GlobalConstantHelpers.APIKEYEXPIRATIONTIME).Select(s => Convert.ToInt32(s.PARAM_VALUE)).FirstOrDefault();

                var externalApp = await context.EXTERNAL_APPLICATION.Where(w => w.EXTERNAL_APPLICATION_ID.ToString() == request.ExternalAppId && w.IS_ACTIVE).FirstAsync(cancellationToken);

                DateTime dtFecha = DateTime.UtcNow;
                ApiKey apikey = new()
                {
                    API_KEY = cryptography.Encrypt(apiKeygenerated) ?? string.Empty,
                    EXPIRATION_TIME = dtFecha.AddDays(iDays),
                    IS_ACTIVE = true,
                    LCOUNT = 0,
                    CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                    CREATED_DATETIME = dtFecha,
                    EXTERNAL_APPLICATION = externalApp
                };

                await context.API_KEY.AddAsync(apikey, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                apiKeydto.ApiKey = apiKeygenerated;
                apiKeydto.DateExpired = apikey.EXPIRATION_TIME;
                apiKeydto.Status = GlobalConstantHelpers.APIKEYSTATUS;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGENERATEAPIKEYEXTERNALAPPS
                   , GlobalConstantHelpers.METHODGENERATEAPIKEYEXTERNALAPP
                   , LogTypeEnum.Error
                   , request.UserIdentification
                   , string.Concat(GlobalConstantMessages.LOGERRORGENERATEAPIEXTERNALAPP, ex.GetException())
                   , GlobalConstantHelpers.NAMECLASSGENERATEAPIKEYEXTERNALAPP);
                throw;
            }

            return apiKeydto;
        }

        /// <summary>
        /// This method get an API Key 
        /// </summary>
        /// <param name="lstSystemParams">System parameters</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>API Key</returns>
        public async Task<string> GetApiKey(List<SystemParam> lstSystemParams, CancellationToken cancellationToken)
        {
            string apiKeygenerated = security.GenerateApiKey(lstSystemParams);
            bool isExists = await context.API_KEY.AnyWithNoLockAsync(w => w.API_KEY == apiKeygenerated, cancellationToken);

            if (!isExists)
            {
                return apiKeygenerated;
            }
            else
            {
                return await GetApiKey(lstSystemParams, cancellationToken);
            }

        }
    }
}

#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-26.
// Comment: DELETE API KEY.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.DeleteApiKey
{
    /// <summary>
    /// Class DTO to delete an API Key, the type of delete is logic
    /// </summary>
    public class DeleteApiKeyCommand : BasePropertiesDto, IRequest<bool>
    {
        /// <summary>
        /// Initial attributes
        /// </summary>
        public DeleteApiKeyCommand()
        {
            ExternalAppId = string.Empty;
            APIKey = string.Empty;
        }

        /// <summary>
        /// GUID Assign to App
        /// </summary>
        public string ExternalAppId { get; set; }

        /// <summary>
        /// API Key generated
        /// </summary>
        public string APIKey { get; set; }
    }

    /// <summary>
    /// Principal class to delete an API Key
    /// </summary>
    public class DeleteApiKeyHandler : IRequestHandler<DeleteApiKeyCommand, bool>
    {
        private readonly ILogService logService;
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor that injects the interface ILogService and ICreateAdministrator.
        /// </summary>            
        /// <param name="logService">Inject the interface necessary for save the log</param>
        /// <param name="context">Inject the interface necessary for create administrators</param>
        public DeleteApiKeyHandler(ILogService logService, IApplicationDbContext context)
        {
            this.logService = logService;
            this.context = context;
        }

        /// <summary>
        /// This method delete an API Key
        /// </summary>
        /// <param name="request">Parameters request</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>logic value</returns>
        public async Task<bool> Handle(DeleteApiKeyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var apiKeys = await context.API_KEY
                                    .Where(w => w.EXTERNAL_APPLICATION_ID.ToString() == request.ExternalAppId
                                        && w.API_KEY == request.APIKey)
                                    .FirstAsync(cancellationToken);

                apiKeys.IS_ACTIVE = false;
                apiKeys.MODIFIED_DATETIME = DateTime.UtcNow;
                apiKeys.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODDELETEAPIKEY
                  , GlobalConstantHelpers.METHODDELETEAPIKEY
                  , LogTypeEnum.Error
                  , request.UserIdentification
                  , string.Concat(GlobalConstantMessages.LOGERRORDELETEAPIKEY, ex.GetException())
                  , GlobalConstantHelpers.NAMECLASSDELETEPIKEY);
                throw;
            }

            return true;
        }
    }
}

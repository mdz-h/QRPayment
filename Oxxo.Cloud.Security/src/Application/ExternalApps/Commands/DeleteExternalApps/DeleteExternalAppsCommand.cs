#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-12-08.
// Comment: Command delete external apps.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.DeleteExternalApps
{
    public class DeleteExternalAppsCommand : BasePropertiesDto, IRequest<bool>
    {
        public DeleteExternalAppsCommand() { ExternalAppId = string.Empty; }

        public string ExternalAppId { get; set; }
    }

    public class DeleteExternalAppsHandler : IRequestHandler<DeleteExternalAppsCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>
        /// <param name="logService">"Inject" the log instance</param>       
        public DeleteExternalAppsHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }

        /// <summary>
        /// Contains all the business rules necessary to delete the external application item.        
        /// </summary>
        /// <param name="request">Contains the external app id to delete.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate the operation result</returns>
        /// <exception cref="CustomException">Contains the internal generated error</exception>
        public async Task<bool> Handle(DeleteExternalAppsCommand request, CancellationToken cancellationToken)
        {
            bool lResult = false;
            try
            {
                var externalApp = await context.EXTERNAL_APPLICATION.Where(w => w.EXTERNAL_APPLICATION_ID.ToString() == request.ExternalAppId && w.IS_ACTIVE).FirstAsync(cancellationToken);

                externalApp.IS_ACTIVE = false;
                externalApp.LCOUNT++;
                externalApp.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                externalApp.MODIFIED_DATETIME = DateTime.UtcNow;

                context.EXTERNAL_APPLICATION.Update(externalApp);
                await context.SaveChangesAsync(cancellationToken);               
                lResult = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODDELETEEXTERNALAPPS,
                    GlobalConstantHelpers.METHODDELETEEXTERNALAPPS,
                    LogTypeEnum.Error,
                    request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORDELETEEXTERNALAPPSCOMMAND, ex.GetException(), request.ExternalAppId),
                    GlobalConstantHelpers.NAMECLASSDELETEEXTERNALAPPSCOMMANDHANDLER);
                throw;
            }

            return lResult;
        }
    }
}

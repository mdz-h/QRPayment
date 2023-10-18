#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-12-06.
// Comment: Command update external apps.
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

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.UpdateExternalApps
{
    /// <summary>
    /// Request DTO Class
    /// </summary>
    public class UpdateExternalAppsCommand : BasePropertiesDto, IRequest<bool>
    {
        /// <summary>
        /// Initial attributes
        /// </summary>
        public UpdateExternalAppsCommand() 
        {
            ExternalAppId = string.Empty;
            Name = string.Empty;
        }
        public string ExternalAppId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }                
    }

    /// <summary>
    /// This method update external applications
    /// </summary>
    public class UpdateExternalAppsHandler : IRequestHandler<UpdateExternalAppsCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>
        /// <param name="logService">"Inject" the log instance</param>       
        public UpdateExternalAppsHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }

        /// <summary>
        /// Contains all the business rules necessary to update the external app item.        
        /// </summary>
        /// <param name="request">Contains the main values for update a external app.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate the operation result</returns>
        /// <exception cref="CustomException">Contains the internal generated error</exception>
        public async Task<bool> Handle(UpdateExternalAppsCommand request, CancellationToken cancellationToken)
        {
            bool lResult = false;
            try
            {               
                var externalApp = await context.EXTERNAL_APPLICATION.Where(w => w.EXTERNAL_APPLICATION_ID.ToString() == request.ExternalAppId).FirstAsync(cancellationToken);

                if (request.IsActive ?? false)
                {
                    externalApp.NAME = request.Name.Equals(externalApp.NAME) ? externalApp.NAME : request.Name;
                    externalApp.LCOUNT++;
                }

                externalApp.IS_ACTIVE = request.IsActive ?? false;
                externalApp.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                externalApp.MODIFIED_DATETIME = DateTime.UtcNow;
                context.EXTERNAL_APPLICATION.Update(externalApp);
                await this.context.SaveChangesAsync(cancellationToken);
               
                lResult = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODUPDATEEXTERNALAPPS,
                    GlobalConstantHelpers.METHODUPDATEEXTERNALAPPS,
                    LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORUPDATEEXTERNALAPPSCOMMAND, ex.GetException(), request.ExternalAppId),
                    GlobalConstantHelpers.NAMECLASSUPDATEEXTERNALAPPSCOMMANDHANDLER);
                throw;
            }
            return lResult;
        }
    }
}

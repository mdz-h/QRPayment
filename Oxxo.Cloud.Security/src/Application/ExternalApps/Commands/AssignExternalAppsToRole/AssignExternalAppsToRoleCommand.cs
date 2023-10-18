#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    2023-01-03.
// Comment: Class of Create relate External application with a role.
//===============================================================================
#endregion

using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.AssignExternalAppsToRole
{
    /// <summary>
    /// This class is a dto to get request parameters
    /// </summary>
    public class AssignExternalAppsToRoleCommand : BasePropertiesDto, IRequest<bool>
    {
        /// <summary>
        /// Initial properties in constructor class
        /// </summary>
        public AssignExternalAppsToRoleCommand() 
        {
            ExternalAppId = string.Empty;
        }

        public int WorkgroupId { get; set; }
        public string ExternalAppId { get; set; }
    }

    /// <summary>
    /// This class generate the relationship between External Application and roles
    /// </summary>
    public class AssignExternalAppsToRoleHandle : IRequestHandler<AssignExternalAppsToRoleCommand, bool>
    {
        private readonly ILogService logService;
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor inject 
        /// <param name="logService">Inject the interface necessary for save the log</param>
        /// <param name="context">Inject the interface necessary for create administrators</param>
        /// </summary>
        public AssignExternalAppsToRoleHandle(ILogService logService, IApplicationDbContext context)
        {
            this.logService = logService;
            this.context = context; 
        }

        /// <summary>
        /// This method process the generate to create the relationship
        /// </summary>
        /// <param name="request">Request parameters</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>logic value</returns>
        public async Task<bool> Handle(AssignExternalAppsToRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                DateTime dtFecha = DateTime.UtcNow;
                UserWorkgroupLink userWorkgroupLink = new()
                {
                    GUID = new Guid(request.ExternalAppId),
                    WORKGROUP_ID = request.WorkgroupId,
                    IS_ACTIVE = true,
                    LCOUNT = 0,
                    CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                    CREATED_DATETIME = dtFecha,
                };

                await context.USER_WORKGROUP_LINK.AddAsync(userWorkgroupLink, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                return true;

            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTPOSTEXTERNALAPPSASSIGN,
                    GlobalConstantHelpers.METHODASSIGNEXTERNALAPP,
                    LogTypeEnum.Error,
                    request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERROREXTERNALAPPTOASSIGN, ex.GetException()),
                    GlobalConstantHelpers.NAMECLASSASSIGNEXTERNALAPP);
                throw;
            }
        }
    }
}

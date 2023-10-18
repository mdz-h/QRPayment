#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    28/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.DeletePermissions
{
    /// <summary>
    /// Dto Class to request the parameters
    /// </summary>
    public class DeletePermissionsCommand : BasePropertiesDto, IRequest<bool>
    {
        public int PermissionID { get; set; }
    }

    /// <summary>
    /// This class update to inactive the permissions
    /// </summary>
    public class DeletePermissionsHandler : IRequestHandler<DeletePermissionsCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the CreatePermissionsHandler.
        /// </summary>
        /// <param name="ILogService">"Inject" the log instance</param>
        /// <param name="context">Inject context</param>
        public DeletePermissionsHandler(ILogService logService, IApplicationDbContext context)
        {
            this.logService = logService;
            this.context = context;
        }

        /// <summary>
        /// Principal method
        /// </summary>
        /// <param name="request">Request parameters</param>
        /// <param name="cancellationToken">CancelationToken</param>
        /// <returns>logic value to indicated the result to operation</returns>
        public async Task<bool> Handle(DeletePermissionsCommand request, CancellationToken cancellationToken)
        {
            bool result = false;
            try
            {
                var permission = await context.PERMISSION.Where(w => w.PERMISSION_ID == request.PermissionID).FirstAsync(cancellationToken);

                permission.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                permission.MODIFIED_DATETIME = DateTime.UtcNow;
                permission.IS_ACTIVE = false;
                permission.LCOUNT++;

                await context.SaveChangesAsync(cancellationToken);
                result = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONDELETE
                    , LogTypeEnum.Error
                    , request.UserIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIDELETE, ex.GetException())
                    , GlobalConstantHelpers.NAMECLASSPERMISSIONDELETECOMMANDHANDLER);
                throw;
            }

            return result;
        }
    }
}

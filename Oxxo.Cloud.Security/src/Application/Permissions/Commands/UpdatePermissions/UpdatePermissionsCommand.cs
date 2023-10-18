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
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.UpdatePermissions
{
    /// <summary>
    /// Class DTO to request parameters
    /// </summary>
    public class UpdatePermissionsCommand : BasePropertiesDto, IRequest<bool>
    {
        /// <summary>
        /// Constructor initial attributes
        /// </summary>
        public UpdatePermissionsCommand()
        {
            Name = string.Empty;
            Description = string.Empty;
            Code = string.Empty;
        }

        public int PermissionID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int ModuleID { get; set; }
        public int PermissionTypeID { get; set; }
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Principal method
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">CancelationToken</param>
    /// <returns>logic value to indicated the result to operation</returns>
    public class UpdatePermissionsHandler : IRequestHandler<UpdatePermissionsCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the CreatePermissionsHandler.
        /// </summary>
        /// <param name="ILogService">"Inject" the log instance</param>
        public UpdatePermissionsHandler(ILogService logService, IApplicationDbContext context)
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
        /// <exception cref="CustomException">Exception</exception>
        public async Task<bool> Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
        {
            bool result = false;
            try
            {
                var permission = await context.PERMISSION.Where(w => w.PERMISSION_ID == request.PermissionID).FirstAsync(cancellationToken);

                if (request.IsActive ?? false)
                {
                    permission.NAME = request.Name;
                    permission.CODE = request.Code;
                    permission.DESCRIPTION = request.Description;
                    permission.MODULE_ID = request.ModuleID;
                    permission.PERMISSION_TYPE_ID = request.PermissionTypeID;
                    permission.LCOUNT++;
                }

                permission.IS_ACTIVE = request.IsActive ?? false;
                permission.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                permission.MODIFIED_DATETIME = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
                result = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONUPDATE
                    , LogTypeEnum.Error
                    , request.UserIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIUPDATE, ex.GetException())
                    , GlobalConstantHelpers.NAMECLASSPERMISSIONUPDATECOMMANDHANDLER);
                throw;
            }

            return result;
        }
    }
}

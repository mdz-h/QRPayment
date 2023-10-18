#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    28/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.CreatePermissions
{
    /// <summary>
    /// This class contains the request parameters
    /// </summary>
    public class CreatePermissionsCommand : BasePropertiesDto, IRequest<bool>
    {
        public CreatePermissionsCommand()
        {
            Name = string.Empty;
            Code = string.Empty;
            Description = string.Empty;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int ModuleID { get; set; }
        public int PermissionTypeID { get; set; }
    }

    /// <summary>
    /// Generate new permissions 
    /// </summary>
    public class CreatePermissionsHandler : IRequestHandler<CreatePermissionsCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the P factory.
        /// </summary>
        /// <param name="ILogService">"Inject" the log instance</param>
        public CreatePermissionsHandler(ILogService logService, IApplicationDbContext context)
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
        public async Task<bool> Handle(CreatePermissionsCommand request, CancellationToken cancellationToken)
        {
            bool lResult;
            try
            {                
                var permission = new Permission()
                {
                    NAME = request.Name ?? string.Empty,
                    CODE = request.Code ?? string.Empty,
                    DESCRIPTION = request.Description ?? string.Empty,
                    MODULE_ID = request.ModuleID,
                    PERMISSION_TYPE_ID = request.PermissionTypeID,
                    IS_ACTIVE = true,
                    LCOUNT = 0,
                    CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                    CREATED_DATETIME = DateTime.UtcNow
                };

                await context.PERMISSION.AddAsync(permission, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                lResult = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONCREATE
                    , LogTypeEnum.Error
                    , request.UserIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPICREATE, ex.GetException())
                    , GlobalConstantHelpers.NAMECLASSPERMISSIONCREATECOMMANDHANDLER);
                throw;
            }

            return lResult;
        }
    }
}

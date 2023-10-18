#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    30/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Queries
{
    /// <summary>
    /// Command Class
    /// </summary>
    public class PermissionsQueryById : BasePropertiesDto, IRequest<PermissionGetDto>
    {
        public int permissionID { get; set; }
    }

    /// <summary>
    /// Principal Class
    /// </summary>
    public class PermissionsQueryByIdHandler : IRequestHandler<PermissionsQueryById, PermissionGetDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor Class
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="logService">Log Service</param>
        public PermissionsQueryByIdHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }

        /// <summary>
        /// Principal Method
        /// </summary>
        /// <param name="request">Params</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>item of permission related id</returns>
        public async Task<PermissionGetDto> Handle(PermissionsQueryById request, CancellationToken cancellationToken)
        {
            PermissionGetDto permissionsGetDTO = new PermissionGetDto();
            try
            {
                var result = await (from perm in context.PERMISSION.AsNoTracking()
                                    join mod in context.MODULE.AsNoTracking()
                                        on perm.MODULE_ID
                                        equals mod.MODULE_ID
                                    join permType in context.PERMISSION_TYPE.AsNoTracking()
                                        on perm.PERMISSION_TYPE_ID
                                        equals permType.PERMISSION_TYPE_ID
                                    where perm.PERMISSION_ID == request.permissionID
                                    select new PermissionGetDto
                                    {
                                        PermissionID = perm.PERMISSION_ID,
                                        PermissionName = perm.NAME ?? string.Empty,
                                        ModuleId = mod.MODULE_ID,
                                        ModuleName = mod.NAME ?? string.Empty,
                                        PermissionTypeID = permType.PERMISSION_TYPE_ID,
                                        PermissionTypeName = permType.NAME ?? string.Empty
                                    }).FirstOrDefaultAsync(cancellationToken);

                if (result == null)
                    throw new CustomException(GlobalConstantMessages.PERMISSIONOTRECORD, HttpStatusCode.NoContent);
                permissionsGetDTO = result;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION, GlobalConstantHelpers.METHODPERMISSIONGETBYID, LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORGETPERMISSIONAPI, ex.InnerException == null ? ex.Message : ex.InnerException.Message), GlobalConstantHelpers.NAMECLASSPERMISSIONGETPERMISSIONS);
                throw;
            }

            return permissionsGetDTO;
        }
    }
}

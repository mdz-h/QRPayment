#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristáin (NEORIS).
// Date:    08/05/2023.
// Comment: Class Get Menu of Front End.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Queries.MenuPerModuleQuery
{
    /// <summary>
    /// Params query
    /// </summary>
    public class MenuPerModuleQuery : BasePropertiesDto, IRequest<List<MenuFrontEndResponse>>
    {
        public MenuPerModuleQuery()
        {            
            ModuleName = string.Empty;
        }
        public Guid UserId { get; set; }
        public string ModuleName { get; set; }
        public int? ParentId { get; set; }
    }

    /// <summary>
    /// Principal Class
    /// </summary>
    public class MenuPerModuleQueryHandler : IRequestHandler<MenuPerModuleQuery, List<MenuFrontEndResponse>>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor Class
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="logService">Log Service</param>
        public MenuPerModuleQueryHandler(IApplicationDbContext context, ILogService logService)
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
        public async Task<List<MenuFrontEndResponse>> Handle(MenuPerModuleQuery request, CancellationToken cancellationToken)
        {            
            try
            {
                int workGroupId = await context.USER_WORKGROUP_LINK.AsNoTracking().Where(u => u.GUID == request.UserId).Select(u => u.WORKGROUP_ID).FirstAsync(cancellationToken);
                if(workGroupId == 0) 
                {
                    throw new CustomException(GlobalConstantMessages.NOWORKGROUPRELATEDUSERID, HttpStatusCode.Forbidden);
                }
                int moduleId = await context.MODULE.Where(m => m.NAME == request.ModuleName && m.IS_ACTIVE).Select(m => m.MODULE_ID).FirstAsync(cancellationToken);
                int permissionTypeId = await context.PERMISSION_TYPE.Where(pt => pt.CODE == GlobalConstantHelpers.PERMISSIONTYPECODEFRONTEND).Select(pt => pt.PERMISSION_TYPE_ID).FirstAsync(cancellationToken);

                List<MenuFrontEndResponse> listOfMenuFrontEndResponse = await context.PERMISSION.AsNoTracking()
                                              .Include(p => p.WORKGROUPPERMISSIONLINK).AsNoTracking()
                                              .Include(p => p.PERMISSIONFRONTEND).AsNoTracking()                                                              
                                              .Where(p => p.PERMISSIONFRONTEND!.Where(pf => request.ParentId == null || pf.PARENT_ID == request.ParentId).Select(pf => pf.PERMISSION_ID).Contains(p.PERMISSION_ID)
                                              && p.MODULE_ID == moduleId
                                              && p.PERMISSION_TYPE_ID == permissionTypeId       
                                              && p.IS_ACTIVE
                                              && p.WORKGROUPPERMISSIONLINK!.Where(w => w.WORKGROUP_ID == workGroupId && w.IS_ACTIVE)
                                                                         .Select(w => w.PERMISSION_ID).Contains(p.PERMISSION_ID)
                                              ).Select(menu => new MenuFrontEndResponse()
                                              { 
                                                  PERMISSION_ID = menu.PERMISSION_ID,
                                                  CODE = menu!.CODE!,
                                                  NAME = menu!.NAME!,
                                                  DESCRIPTION = menu!.DESCRIPTION!,
                                                  ICON = menu!.PERMISSIONFRONTEND!.First(pf => pf.PERMISSION_ID == menu.PERMISSION_ID).ICON,
                                                  PARENT_ID = menu.PERMISSIONFRONTEND!.First(pf => pf.PERMISSION_ID == menu.PERMISSION_ID).PARENT_ID,
                                                  SORT_ORDER = menu.PERMISSIONFRONTEND!.First(pf => pf.PERMISSION_ID == menu.PERMISSION_ID).SORT_ORDER,
                                                  QUICK_ACCESS = menu.PERMISSIONFRONTEND!.First(pf => pf.PERMISSION_ID == menu.PERMISSION_ID).QUICK_ACCESS,
                                                  SUBMENUS = context.PERMISSION_FRONTEND.AsNoTracking().Where(p => p.PARENT_ID == menu.PERMISSION_ID).Any()

                                              }).ToListWithNoLockAsync();
                if (!listOfMenuFrontEndResponse.Any())
                {
                    throw new CustomException(GlobalConstantMessages.NORECORD, HttpStatusCode.NoContent);
                }
                return listOfMenuFrontEndResponse;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION, GlobalConstantHelpers.METHODPERMISSIONGETMENUPERMODULE, LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORMENUPERMODULE, ex.InnerException == null ? ex.Message : ex.InnerException.Message), GlobalConstantHelpers.NAMECLASSMENUFERMODULEQUERY);
                throw;
            }            
        }
    }
}

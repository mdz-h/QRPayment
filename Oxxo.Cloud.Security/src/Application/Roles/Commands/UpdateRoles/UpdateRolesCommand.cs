#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-28.
// Comment: Command update workgroup.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.UpdateRoles
{
    public class UpdateRolesCommand : BasePropertiesDto, IRequest<bool>
    {
        public int RoleId { get; set; }
        public string ShortName { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public string Description { get; set; } = string.Empty;

    }

    public class UpdateRolesHandler : IRequestHandler<UpdateRolesCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>
        /// <param name="logService">"Inject" the log instance</param>       
        public UpdateRolesHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }

        /// <summary>
        /// Contains all the business rules necessary to create the workgroup item.        
        /// </summary>
        /// <param name="request">Contains the main values for a workgroup.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate the operation result</returns>
        /// <exception cref="CustomException">Contains the internal generated error</exception>
        public async Task<bool> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
        {
            bool lResult = false;
            try
            {                
                var role = await context.WORKGROUP.FirstOrDefaultWithNoLockAsync(w => w.WORKGROUP_ID == request.RoleId, cancellationToken);
                if (role == null)
                {
                    throw new CustomException(GlobalConstantMessages.ROLENOTFOUND, HttpStatusCode.UnprocessableEntity);
                }

                role.IS_ACTIVE = request.IsActive ?? false;
                if (role.IS_ACTIVE)
                {
                    role.NAME = request.ShortName.Equals(role.NAME) ? role.NAME : request.ShortName;
                    role.DESCRIPTION = request.Description.Equals(role.DESCRIPTION) ? role.DESCRIPTION : request.Description;
                }
                role.LCOUNT = role.LCOUNT + 1;
                role.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                role.MODIFIED_DATETIME = DateTime.UtcNow;

                await this.context.SaveChangesAsync(cancellationToken);               
                lResult = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODUPDATEWORKGROUP, GlobalConstantHelpers.METHODUPDATEWORKGROUP, LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORUPDATEWORKGROUPCOMMAND, ex.InnerException == null ? ex.Message : ex.InnerException.Message, request.RoleId), GlobalConstantHelpers.NAMECLASSUPDATEROLECOMMANDHANDLER);
                throw;
            }
            return lResult;
        }
    }
}

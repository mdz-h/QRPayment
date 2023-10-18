#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-29.
// Comment: Command logical delete workgroup.
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

namespace Oxxo.Cloud.Security.Application.Roles.Commands.DeleteRoles
{
    public class DeleteRolesCommand : BasePropertiesDto, IRequest<bool>
    {
        public int RoleId { get; set; }
    }

    public class DeleteRoleHandler : IRequestHandler<DeleteRolesCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>
        /// <param name="logService">"Inject" the log instance</param>       
        public DeleteRoleHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }

        /// <summary>
        /// Contains all the business rules necessary to delete the workgroup item.        
        /// </summary>
        /// <param name="request">Contains the main values for a workgroup.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate the operation result</returns>
        /// <exception cref="CustomException">Contains the internal generated error</exception>
        public async Task<bool> Handle(DeleteRolesCommand request, CancellationToken cancellationToken)
        {
            bool lResult = false;
            try
            {
                var role = await context.WORKGROUP.Where(w => w.WORKGROUP_ID == request.RoleId).FirstOrDefaultAsync(cancellationToken);
                if (role == null)
                {
                    throw new CustomException(GlobalConstantMessages.ROLENOTFOUND, HttpStatusCode.UnprocessableEntity);
                }
                role.IS_ACTIVE = false;
                role.LCOUNT = role.LCOUNT + 1;
                role.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                role.MODIFIED_DATETIME = DateTime.UtcNow;

                await this.context.SaveChangesAsync(cancellationToken);                
                lResult = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODDELETEWORKGROUP, GlobalConstantHelpers.METHODDELETEWORKGROUP, LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORDELETEWORKGROUPCOMMAND, ex.InnerException == null ? ex.Message : ex.InnerException.Message, request.RoleId), GlobalConstantHelpers.NAMECLASSDELETEROLECOMMANDHANDLER);
                throw;
            }
            return lResult;
        }
    }
}

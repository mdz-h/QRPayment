#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-29.
// Comment: Command logical delete workgroup to user.
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

namespace Oxxo.Cloud.Security.Application.Roles.Commands.DeleteRoles
{
    public class DeleteUserRoleCommand : BasePropertiesDto, IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>
        /// <param name="logService">"Inject" the log instance</param>       
        public DeleteUserRoleCommandHandler(IApplicationDbContext context, ILogService logService)
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
        public async Task<bool> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            bool lResult = false;
            try
            {
                var userWorkgroupLink = await context.USER_WORKGROUP_LINK.FirstOrDefaultWithNoLockAsync(w => w.GUID.ToString() == request.UserId, cancellationToken);
                if (userWorkgroupLink == null)
                {
                    throw new CustomException(GlobalConstantMessages.USERWORKGROUPLINKNOTFOUND, HttpStatusCode.UnprocessableEntity);
                }
                userWorkgroupLink.IS_ACTIVE = false;
                userWorkgroupLink.LCOUNT = userWorkgroupLink.LCOUNT + 1;
                userWorkgroupLink.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                userWorkgroupLink.MODIFIED_DATETIME = DateTime.UtcNow;
                await this.context.SaveChangesAsync(cancellationToken);               
                lResult = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODDELETEWORKGROUPUSER, GlobalConstantHelpers.METHODDELETEWORKGROUPUSER, LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORDELETEUSERWORKGROUPCOMMAND, ex.InnerException == null ? ex.Message : ex.InnerException.Message, request.UserId), GlobalConstantHelpers.NAMECLASSDELETEUSERROLECOMMANDHANDLER);
                throw;
            }
            return lResult;
        }
    }
}

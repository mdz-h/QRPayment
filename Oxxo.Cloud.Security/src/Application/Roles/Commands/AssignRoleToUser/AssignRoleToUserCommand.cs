#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-12-08.
// Comment: Assign Role To User.
//===============================================================================
#endregion
using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommand : BasePropertiesDto, IRequest<bool>
    {
        public string? Guid { get; set; }
        public int WorkgroupId { get; set; }
    }

    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;
        public AssignRoleToUserCommandHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }
        public async Task<bool> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var assigment = new UserWorkgroupLink()
                {
                    GUID = new Guid(request.Guid!),
                    WORKGROUP_ID = request.WorkgroupId,
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                    CREATED_DATETIME = DateTime.UtcNow,
                    MODIFIED_BY_OPERATOR_ID = null,
                    MODIFIED_DATETIME = null

                };
                await context.USER_WORKGROUP_LINK.AddAsync(assigment, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODENABLEDDEVICE, GlobalConstantHelpers.METHODENABLED,
                    LogTypeEnum.Error, request.UserIdentification, string.Concat(GlobalConstantMessages.LOGERRORASSIGNTROLTOUSERCOMMAND, ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex.StackTrace), GlobalConstantHelpers.NAMEENABLEDDEVICECONTROLLER);
                throw;
            }
        }

    }
}

#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-12-06.
// Comment: Command Assigment Permission to Role.
//===============================================================================
#endregion
using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.AssigmentPermissionstorole
{
    public class AssigmentPermissionToRoleCommand : BasePropertiesDto, IRequest<bool>
    {
        public int WorkgroupId { get; set; }
        public List<int> PermissionId { get; set; }
        public AssigmentPermissionToRoleCommand()
        {
            WorkgroupId = 0;
            PermissionId = new List<int>();
        }
    }

    public class AssigmentPermissionToRoleCommandHandler : IRequestHandler<AssigmentPermissionToRoleCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        public AssigmentPermissionToRoleCommandHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }

        /// <summary>
        /// main method
        /// </summary>
        /// <param name="request">Request app</param>
        /// <param name="cancellationToken">Cancellationtoken</param>
        /// <returns></returns>
        /// <exception cref="CustomException">Excepction</exception>
        public async Task<bool> Handle(AssigmentPermissionToRoleCommand request, CancellationToken cancellationToken)
        {
            bool lResult = false;
            try
            {
                List<WorkgroupPermissionLink> workgroupPermissionLinks = new();

                request.PermissionId.ForEach(f =>
                    {
                        workgroupPermissionLinks.Add(
                            new WorkgroupPermissionLink()
                            {
                                WORKGROUP_ID = request.WorkgroupId,
                                PERMISSION_ID = f,
                                IS_ACTIVE = true,
                                LCOUNT = 0,
                                CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                                CREATED_DATETIME = DateTime.UtcNow

                            });
                    }
                );

                await context.WORKGROUP_PERMISSION_LINK.AddRangeAsync(workgroupPermissionLinks, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
                lResult = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODENABLEDDEVICE,
                    GlobalConstantHelpers.METHODENABLED, LogTypeEnum.Error,
                    request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORASSIGNPERMISSIONTOROLCOMMAND,
                    ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex.StackTrace),
                    GlobalConstantHelpers.NAMEENABLEDDEVICECONTROLLER);
                throw;
            }
            return lResult;
        }
    }
}

#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-25.
// Comment: Class Roles By User ID query.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesByUserIdQuery : BasePropertiesDto, IRequest<RolesDto>
    {
        public string UserId { get; set; } = string.Empty;

        public class RolesByUserIdQueryHandler : IRequestHandler<RolesByUserIdQuery, RolesDto>
        {
            private readonly IApplicationDbContext context;
            private readonly ILogService logService;

            /// <summary>
            /// Constructor that injects the application db context and log instance.
            /// </summary>
            /// <param name="context">"Inject" the application context instance</param>
            /// <param name="logService">"Inject" the log instance</param>       
            public RolesByUserIdQueryHandler(IApplicationDbContext context, ILogService logService)
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
            public async Task<RolesDto> Handle(RolesByUserIdQuery request, CancellationToken cancellationToken)
            {
                RolesDto result = new RolesDto();
                try
                {                    
                    Workgroup? rol = await (from link in context.USER_WORKGROUP_LINK.Where(x => x.GUID.ToString() == request.UserId)
                                     .Include(t => t.WORKGROUP)
                                            select link.WORKGROUP).FirstOrDefaultAsync(cancellationToken);
                    if (rol != null)
                    {
                        result = new RolesDto
                        {
                            WorkgroupId = rol.WORKGROUP_ID,
                            Name = rol.NAME,
                            Description = rol.DESCRIPTION,
                            Code = rol.CODE,
                            IsActive = rol.IS_ACTIVE,
                            Lcount = rol.LCOUNT,
                            CreatedByOperatorId = rol.CREATED_BY_OPERATOR_ID,
                            CreatedDateTime = rol.CREATED_DATETIME,
                            ModifiedByOperatorId = rol.MODIFIED_BY_OPERATOR_ID,
                            ModifiedDateTime = rol.MODIFIED_DATETIME
                        };
                    }
                    else
                    {
                        throw new CustomException(GlobalConstantMessages.ROLENOTFOUND, HttpStatusCode.NoContent);
                    }
                }
                catch (Exception ex)
                {
                    await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETWORKGROUP, GlobalConstantHelpers.METHODGETWORKGROUPBYUSERID, LogTypeEnum.Error, request.UserIdentification, string.Concat(GlobalConstantMessages.LOGERRORGETWORKGROUPQUERY, ex.InnerException == null ? ex.Message : ex.InnerException.Message, request.UserId), GlobalConstantHelpers.NAMECLASSGETROLEBYUSERQUERY);
                    throw;
                }
                return result;
            }
        }
    }
}

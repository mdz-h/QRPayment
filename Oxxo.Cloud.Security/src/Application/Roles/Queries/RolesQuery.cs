#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-25.
// Comment: Class Roles query.
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
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesQuery : BasePropertiesDto, IRequest<List<RolesDto>>
    {
        public int Skip { get; set; }
        public int Take { get; set; }

        public class RolesQueryHandler : IRequestHandler<RolesQuery, List<RolesDto>>
        {
            private readonly IApplicationDbContext context;
            private readonly ILogService logService;

            /// <summary>
            /// Constructor that injects the application db context and log instance.
            /// </summary>
            /// <param name="context">"Inject" the application context instance</param>
            /// <param name="logService">"Inject" the log instance</param>       
            public RolesQueryHandler(IApplicationDbContext context, ILogService logService)
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
            public async Task<List<RolesDto>> Handle(RolesQuery request, CancellationToken cancellationToken)
            {
                List<RolesDto> result = new List<RolesDto>();
                try
                {                   
                    List<Workgroup> roles = await this.context.WORKGROUP.Skip(request.Skip - 1).Take(request.Take).ToListAsync(cancellationToken);
                    if (roles != null && roles.Any())
                    {
                        foreach (Workgroup rol in roles)
                        {
                            result.Add(new RolesDto
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
                            });
                        }
                    }
                    else
                    {
                        throw new CustomException(GlobalConstantMessages.ROLENOTFOUND, HttpStatusCode.NoContent);
                    }                   
                }
                catch (Exception ex)
                {
                    await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETWORKGROUP, GlobalConstantHelpers.METHODGETWORKGROUP, LogTypeEnum.Error, request.UserIdentification,
                        string.Concat(GlobalConstantMessages.LOGERRORGETWORKGROUPQUERY, ex.InnerException == null ? ex.Message : ex.InnerException.Message, request.Skip, request.Take), GlobalConstantHelpers.NAMECLASSGETROLEQUERY);
                    throw;
                }
                return result;
            }
        }

    }
}

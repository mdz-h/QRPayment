#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    14/12/2022.
// Comment: Query Administrators.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Administrators.Queries
{
    /// <summary>
    /// Request DTO Class
    /// </summary>
    public class AdministratorsQuery : BasePropertiesGetDto, IRequest<List<AdministratorGetDto>>
    {
    }

    /// <summary>
    /// Process query
    /// </summary>
    public class AdministratorsQueryHandler : IRequestHandler<AdministratorsQuery, List<AdministratorGetDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor Class
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="logService">Log Service</param>
        public AdministratorsQueryHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }

        /// <summary>
        /// This method exec database and get information
        /// </summary>
        /// <param name="request">request parameters</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>List active Administrators</returns>
        public async Task<List<AdministratorGetDto>> Handle(AdministratorsQuery request, CancellationToken cancellationToken)
        {
            List<AdministratorGetDto> lstAdministrators = new();
            try
            {
                lstAdministrators = await (from item in context.PERSON.AsNoTracking().Where(w => w.IS_ACTIVE)
                                    .Include(i => i.OPERATOR)
                                           let workkgroups = context.USER_WORKGROUP_LINK.AsNoTracking()
                                                                          .Include(i => i.WORKGROUP)
                                                                      .Where(a => a.GUID == item.OPERATOR.OPERATOR_ID && a.IS_ACTIVE).Select(s => s.WORKGROUP).ToList()
                                           where workkgroups.Any()
                                           select new AdministratorGetDto
                                           {
                                               UserId = item.OPERATOR.OPERATOR_ID.ToString(),
                                               UserName = item.NAME ?? string.Empty,
                                               LastnamePat = item.LASTNAME_PAT ?? string.Empty,
                                               LastnameMat = item.LASTNAME_MAT ?? string.Empty,
                                               Email = item.EMAIL ?? string.Empty,
                                               Workgroups = workkgroups
                                           }).Skip(request.PageNumber - 1).Take(request.ItemsNumber).ToListWithNoLockAsync();

                if (!lstAdministrators.Any())
                    throw new CustomException(GlobalConstantMessages.NOTRECORD, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSGET
                    , LogTypeEnum.Error
                    , request.UserIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORGETADMINISTRATORSAPIGET, ex.GetException())
                    , GlobalConstantHelpers.METHOADMINISTRATORSHANDLERGET);
                throw;
            }

            return lstAdministrators;
        }
    }
}

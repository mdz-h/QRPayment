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
using System.Net;

namespace Oxxo.Cloud.Security.Application.Administrators.Queries
{
    /// <summary>
    /// Request DTO Class
    /// </summary>
    public class AdministratorsQueryById : BasePropertiesGetDto, IRequest<AdministratorGetDto>
    {
        /// <summary>
        /// Initial attributes
        /// </summary>
        public AdministratorsQueryById()
        {
            UserId = string.Empty;
        }

        public string UserId { get; set; }
    }

    /// <summary>
    /// Process Query
    /// </summary>
    public class AdministratorsQueryByIdHandler : IRequestHandler<AdministratorsQueryById, AdministratorGetDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor Class
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="logService">Log Service</param>
        public AdministratorsQueryByIdHandler(IApplicationDbContext context, ILogService logService)
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
        public async Task<AdministratorGetDto> Handle(AdministratorsQueryById request, CancellationToken cancellationToken)
        {
            List<AdministratorGetDto> lstAdministrator = new();
            try
            {
                lstAdministrator = await (from item in context.PERSON.Where(w => w.IS_ACTIVE)
                                            .Include(i => i.OPERATOR)
                                          let workkgroups = context.USER_WORKGROUP_LINK
                                                                .Include(i => i.WORKGROUP)
                                                            .Where(a => a.GUID == item.OPERATOR.OPERATOR_ID && a.IS_ACTIVE).Select(s => s.WORKGROUP).ToList()

                                          where workkgroups.Any() && item.OPERATOR.OPERATOR_ID.ToString() == request.UserId
                                          select new AdministratorGetDto
                                          {
                                              UserId = item.OPERATOR.OPERATOR_ID.ToString(),
                                              UserName = item.NAME ?? string.Empty,
                                              LastnamePat = item.LASTNAME_PAT ?? string.Empty,
                                              LastnameMat = item.LASTNAME_MAT ?? string.Empty,
                                              Email = item.EMAIL ?? string.Empty,
                                              Workgroups = workkgroups
                                          }).ToListAsync(cancellationToken);

                if (!lstAdministrator.Any())
                    throw new CustomException(GlobalConstantMessages.NOTRECORD, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSGETBYID
                    , LogTypeEnum.Error
                    , request.UserIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORGETADMINISTRATORSAPIGET, ex.GetException())
                    , GlobalConstantHelpers.METHOADMINISTRATORSHANDLERGETBYID);
                throw;
            }

            return lstAdministrator.First();
        }
    }
}

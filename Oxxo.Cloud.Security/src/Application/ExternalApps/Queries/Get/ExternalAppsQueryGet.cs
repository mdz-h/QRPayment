#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-13.
// Comment: Class of ExternalApps Query.
//===============================================================================
#endregion

using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Infrastructure.Extensions;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Queries.Get
{
    public class ExternalAppsQueryGet : IExternalAppsQueryGet
    {
        #region Properties
        /// <summary>
        /// Contract of ApplicationDbContext
        /// </summary>
        private readonly IApplicationDbContext context;
        private readonly ICryptographyService cryptography;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">context of ApplicationDbContext</param>
        public ExternalAppsQueryGet(IApplicationDbContext context, ICryptographyService cryptography)
        {
            this.context = context;
            this.cryptography = cryptography;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Get data of ExternalApps
        /// </summary>
        /// <param name="request">Filter</param>
        public async Task<List<ExternalAppsResponse>> Get(GetExternalAppsQuery request)
        {
            List<ExternalAppsResponse> data;

            data = await (from extapp in context.EXTERNAL_APPLICATION.AsNoTracking()
                            .Include(i => i.APIKEYS)
                          let workkgroups = context.USER_WORKGROUP_LINK.AsNoTracking()
                                                                        .Include(i => i.WORKGROUP)
                                                                    .Where(a => a.GUID == extapp.EXTERNAL_APPLICATION_ID && a.IS_ACTIVE).Select(s => s.WORKGROUP).ToList()
                          where
                             extapp.IS_ACTIVE
                             && (string.IsNullOrEmpty(request.Identifier) || extapp.EXTERNAL_APPLICATION_ID.ToString() == request.Identifier) 
                          select new ExternalAppsResponse
                          {
                              External_Aplication_Id = extapp.EXTERNAL_APPLICATION_ID,
                              Name = extapp.NAME ?? string.Empty,
                              Code = extapp.CODE ?? string.Empty,
                              Is_active = extapp.IS_ACTIVE,
                              Created_datetime = extapp.CREATED_DATETIME,
                              Created_by_operator_id = extapp.CREATED_BY_OPERATOR_ID,
                              Modified_datetime = extapp.MODIFIED_DATETIME,
                              Modified_by_operator_id = extapp.MODIFIED_BY_OPERATOR_ID,
                              Lcount = extapp.LCOUNT,
                              Workgroups = workkgroups,
                              ApiKeys = extapp.APIKEYS == null
                                        ? new List<ExternalAppApiKeyDto>()
                                        : (from item in extapp.APIKEYS
                                           select new ExternalAppApiKeyDto
                                           {
                                               Api_Key_Id = item.API_KEY_ID,
                                               External_application_Id = item.EXTERNAL_APPLICATION_ID,
                                               Api_Key = cryptography.Decrypt(item.API_KEY ?? string.Empty),
                                               Expiration_Time = item.EXPIRATION_TIME,
                                               Is_Active = item.IS_ACTIVE,
                                               Lcount = item.LCOUNT,
                                               Created_By_Operator_Id = item.CREATED_BY_OPERATOR_ID,
                                               Created_DateTime = item.CREATED_DATETIME,
                                               Modified_By_Operator_Id = item.MODIFIED_BY_OPERATOR_ID,
                                               Modified_DateTime = item.MODIFIED_DATETIME,
                                           }).ToList()
                          }).Skip((request.PageNumber - 1) * request.ItemsNumber).Take(request.ItemsNumber).ToListWithNoLockAsync(); 
            return data;
        }

        /// <summary>
        /// Validate if exists rows
        /// </summary>
        /// <param name="request">Data filter</param>
        public async Task<bool> ValidateIfExists(GetExternalAppsQuery request)
        {
            return await context.EXTERNAL_APPLICATION.AsNoTracking()
                    .Where(extapp => extapp.IS_ACTIVE
                        && (!string.IsNullOrEmpty(request.Identifier) ? extapp.EXTERNAL_APPLICATION_ID.ToString() == request.Identifier : true))
                    .Skip((request.PageNumber - 1) * request.ItemsNumber).Take(request.ItemsNumber).AnyWithNoLockAsync();
        }
        #endregion
    }
}

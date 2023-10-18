#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-13.
// Comment: Interface of ExternalApps Query.
//===============================================================================
#endregion

using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Application.ExternalApps.Queries.Get;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces
{
    public interface IExternalAppsQueryGet
    {
        /// <summary>
        /// Get data of ExternalApps
        /// </summary>
        /// <param name="request">Filter</param>
        Task<List<ExternalAppsResponse>> Get(GetExternalAppsQuery request);

        /// <summary>
        /// Validate if exists data
        /// </summary>
        /// <param name="request">Data to filter</param>
        Task<bool> ValidateIfExists(GetExternalAppsQuery request);
    }
}
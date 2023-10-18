#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-13.
// Comment: Class Model of ExternalApps Response.
//===============================================================================
#endregion

using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Common.Models
{
    public class ExternalAppsResponse
    {
        public ExternalAppsResponse()
        {
            Code = string.Empty;
            Name = string.Empty;
            ApiKeys = new List<ExternalAppApiKeyDto>();
            Workgroups = new List<Workgroup>();
        }

        public Guid External_Aplication_Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Is_active { get; set; }
        public DateTime Created_datetime { get; set; }
        public Guid Created_by_operator_id { get; set; }
        public DateTime? Modified_datetime { get; set; }
        public Guid? Modified_by_operator_id { get; set; }
        public int Lcount { get; set; }

        public List<ExternalAppApiKeyDto> ApiKeys { get; set; }
        public List<Workgroup> Workgroups { get; set; }
    }
}
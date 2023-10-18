#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    14/12/2022.
// Comment: Query Administrators.
//===============================================================================
#endregion


using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    /// <summary>
    /// DTO Class to get administrators.
    /// </summary>
    public class AdministratorGetDto
    {
        /// <summary>
        /// Constructor class: initial the attributes
        /// </summary>
        public AdministratorGetDto()
        {
            UserId = string.Empty;
            UserName = string.Empty;
            LastnamePat = string.Empty;
            LastnameMat = string.Empty;
            Email = string.Empty;
            Workgroups = new List<Workgroup>();
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string LastnamePat { get; set; }
        public string LastnameMat { get; set; }
        public string Email { get; set; }
        public List<Workgroup> Workgroups { get; set; }
    }
}
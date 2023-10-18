#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-21.
// Comment: GENERATE API KEY.
//===============================================================================
#endregion

namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    /// <summary>
    /// DTO Class 
    /// </summary>
    public class ApiKeyDto
    {
        public ApiKeyDto()
        {
            ApiKey = string.Empty;
            Status = string.Empty;
        }

        public DateTime DateExpired { get; set; }
        public string ApiKey { get; set; }
        public string Status { get; set; }
    }
}

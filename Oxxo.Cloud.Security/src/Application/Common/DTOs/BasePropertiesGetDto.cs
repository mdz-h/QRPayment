#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    09/12/2022.
// Comment: Class DTO.
//===============================================================================
#endregion

using System.Text.Json.Serialization;

namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    /// <summary>
    /// Dto Class
    /// </summary>
    public class BasePropertiesGetDto : BasePropertiesDto
    {
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }
        [JsonPropertyName("itemsNumber")]
        public int ItemsNumber { get; set; }
    }
}

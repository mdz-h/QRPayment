#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristáin (NEORIS).
// Date:    08/05/2023.
// Comment: Class MenuFrontEndResponse
//===============================================================================
#endregion

using System.Text.Json.Serialization;

namespace Oxxo.Cloud.Security.Application.Common.Models
{
    /// <summary>
    /// Class response Menu Front End
    /// </summary>
    public class MenuFrontEndResponse
    {
        /// <summary>
        /// Constructor Method
        /// </summary>
        public MenuFrontEndResponse()
        {
            CODE = string.Empty;
            NAME = string.Empty;
            DESCRIPTION = string.Empty;
            ICON = string.Empty;
            QUICK_ACCESS = string.Empty;
        }
        [JsonPropertyName("permission_id")]
        public int PERMISSION_ID { get; set; }
        [JsonPropertyName("code")]
        public string CODE { get; set; }
        [JsonPropertyName("name")]
        public string NAME { get; set; }
        [JsonPropertyName("description")]
        public string DESCRIPTION { get; set; }
        [JsonPropertyName("icon")]
        public string ICON { get; set; }
        [JsonPropertyName("parent_id")]
        public int PARENT_ID { get; set; }
        [JsonPropertyName("sort_order")]
        public int SORT_ORDER { get; set; }
        [JsonPropertyName("quick_access")]
        public string QUICK_ACCESS { get; set; }
        [JsonPropertyName("submenus")]
        public bool SUBMENUS { get; set; }


    }
}

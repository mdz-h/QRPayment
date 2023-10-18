#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-21.
// Comment: GENERATE API KEY.
//===============================================================================
#endregion

namespace Oxxo.Cloud.Security.Domain.Entities
{
    /// <summary>
    /// Entity class
    /// </summary>
    public class ApiKey : BaseEntity
    {
        public ApiKey() 
        {
            API_KEY = string.Empty;   
        }

        public int API_KEY_ID { get; set; }
        public Guid EXTERNAL_APPLICATION_ID { get; set; }
        public string? API_KEY { get; set; }
        public DateTime EXPIRATION_TIME { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
        public ExternalApplication? EXTERNAL_APPLICATION { get; set; }

    }
}

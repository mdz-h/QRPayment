#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
// Comment: Administrators.
//===============================================================================
#endregion


namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class Person : BaseEntity
    {
        public int PERSON_ID { get; set; }
        public string NAME { get; set; } = string.Empty;
        public string? MIDDLE_NAME { get; set; }
        public string LASTNAME_PAT { get; set; } = string.Empty;
        public string LASTNAME_MAT { get; set; } = string.Empty;
        public string? EMAIL { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }

        public Operator OPERATOR { get; set; }
    }
}

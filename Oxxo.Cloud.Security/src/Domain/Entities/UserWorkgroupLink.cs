#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-29.
// Comment: Entitie User Workgroup Link.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class UserWorkgroupLink : BaseEntity
    {
        public int USER_WORKGROUP_LINK_ID { get; set; }
        public Guid GUID { get; set; }
        public int WORKGROUP_ID { get; set; }
        public Workgroup WORKGROUP { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
    }
}

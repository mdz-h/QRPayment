#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-23.
// Comment: Entitie workgroup.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class Workgroup : BaseEntity
    {
        public Workgroup()
        {
            USER_WORKGROUP_LINKS = new List<UserWorkgroupLink>();
            WORKGROUP = new List<WorkgroupPermissionLink>();
        }
        public int WORKGROUP_ID { get; set; }
        public string? CODE { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
        public List<UserWorkgroupLink> USER_WORKGROUP_LINKS { get; set; }
        public List<WorkgroupPermissionLink> WORKGROUP { get; set; }

    }
}

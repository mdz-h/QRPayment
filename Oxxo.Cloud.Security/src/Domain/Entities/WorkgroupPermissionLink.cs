#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-11-29.
// Comment: Entity Permission Type.
//===============================================================================
#endregion

namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class WorkgroupPermissionLink : BaseEntity
    {
        public int WORKGROUP_PERMISSION_LINK_ID { get; set; }
        public int WORKGROUP_ID { get; set; }
        public Workgroup? WORKGROUP { get; set; }
        public int PERMISSION_ID { get; set; }
        public Permission? PERMISSION { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
    }
}

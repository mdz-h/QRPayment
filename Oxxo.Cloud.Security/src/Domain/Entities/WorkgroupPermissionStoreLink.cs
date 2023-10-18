#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-08.
// Comment: Entity WorkgroupPermissionStoreLink
//===============================================================================
#endregion


namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class WorkgroupPermissionStoreLink : BaseEntity
    {
        public int WORKGROUP_PERMISSION_STORE_LINK_ID { get; set; }
        public int WORKGROUP_PERMISSION_LINK_ID { get; set; }
        public int STORE_PLACE_ID { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
    }
}

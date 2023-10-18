#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-11-23.
// Comment: Entity Permission.
//===============================================================================
#endregion


namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public int PERMISSION_ID { get; set; }
        public int PERMISSION_TYPE_ID { get; set; }        
        public int MODULE_ID { get; set; }        
        public string? CODE { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
        public virtual List<WorkgroupPermissionLink>? WORKGROUPPERMISSIONLINK { get; set; }
        public virtual List<PermissionFrontEnd>? PERMISSIONFRONTEND { get; set; }

    }
}

#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristáin (NEORIS).
// Date:    08/05/2023.
// Comment: Class PermissionFrontEnd
//===============================================================================
#endregion


namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class PermissionFrontEnd : BaseEntity
    {
        public PermissionFrontEnd()
        {
            ICON = string.Empty;            
        }
        public int PERMISSION_FRONTEND_ID { get; set; }
        public int PERMISSION_ID { get; set; }
        public virtual Permission? PERMISSIONS { get; set; }
        public string ICON { get; set; }
        public int PARENT_ID { get; set; }
        public int SORT_ORDER { get; set; }
        public string? QUICK_ACCESS { get; set; }
        public bool FL { get; set; }
        public int LCNT { get; set; }
        public Guid ID_OPR_CRT { get; set; }
        public DateTime TM_CRT { get; set; }
        public Guid? ID_OPR_MDY { get; set; }
        public DateTime? TM_MDY { get; set; }
    }
}

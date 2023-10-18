namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    /// <summary>
    /// Class DTO To Get Permissions
    /// </summary>
    public class PermissionGetDto
    {
        /// <summary>
        /// Constructor Method
        /// </summary>
        public PermissionGetDto()
        {
            this.PermissionID = 0;
            this.PermissionName = string.Empty;
            Description = string.Empty;
            this.ModuleId = 0;
            this.ModuleName = string.Empty;
            this.PermissionTypeID = 0;
            this.PermissionTypeName = string.Empty;
        }

        public int PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int PermissionTypeID { get; set; }
        public string PermissionTypeName { get; set; }
    }
}

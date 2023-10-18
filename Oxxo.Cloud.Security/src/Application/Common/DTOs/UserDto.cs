#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2023-01-25.
// Comment: Dto user.
//===============================================================================
#endregion


namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    public class UserDto : AuthenticateDto
    {
        public UserDto() {
            User = string.Empty;
            Password = string.Empty;
            CrPlace = string.Empty;
            CrStore = string.Empty;
            Till = string.Empty;
            AdministrativeDate = string.Empty;
            ProcessDate = string.Empty;
            Type = string.Empty;
            IsInternal = true;           
            WorkGroup = string.Empty;
            StatusOperator = string.Empty;
            FullName = string.Empty;
        }
        public string User { get; set; }
        public string Password { get; set; }
        public string CrPlace { get; set; }
        public string CrStore { get; set; }
        public string Till { get; set; }
        public string AdministrativeDate { get; set; }
        public string ProcessDate { get; set; }
        public string Type { get; set; }
        public bool IsInternal { get; set; }       
        public string WorkGroup { get; set; }
        public string StatusOperator { get; set; }
        public string FullName { get; set; }

    }
}

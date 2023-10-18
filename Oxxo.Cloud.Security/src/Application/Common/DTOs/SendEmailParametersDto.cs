#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-26.
// Comment: Dto SendEmailParameters.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    public class SendEmailParametersDto
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}

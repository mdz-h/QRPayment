#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-26.
// Comment: Dto email.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    public class EmailDto
    {
        public int TemplateId { get; set; }
        public SendEmailParametersDto Parameters { get; set; } = new();
        public string[] ReceiverEmails { get; set; } = {string.Empty};
    }
}

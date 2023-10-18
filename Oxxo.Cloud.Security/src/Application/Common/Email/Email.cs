#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-26.
// Comment: Class that implements interface IEmail
//===============================================================================
#endregion
using Newtonsoft.Json;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net.Http.Headers;
using GlobalConstantHelpers = Oxxo.Cloud.Security.Domain.Consts.GlobalConstantHelpers;

namespace Oxxo.Cloud.Security.Application.Common.Email
{
    public class Email : IEmail
    {
        private readonly HttpClient client;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor.  
        /// </summary>
        public Email(ILogService logService)
        {
            client = new HttpClient();
            this.logService = logService;
        }

        /// <summary>
        /// This function is responsible that execute the send email
        /// </summary>
        /// <param name="email"></param>        
        /// <param name="token"></param>        
        /// <returns>Bolean</returns>
        public async Task<bool> SendEmail(EmailDto email, string token)
        {
            try
            {
                string emailObject = JsonConvert.SerializeObject(email);

                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS, GlobalConstantHelpers.METHODSENDEMAIL, LogTypeEnum.Information
                , $"{GlobalConstantMessages.LOGINITSENDEMAIL} {emailObject}", GlobalConstantHelpers.NAMECLASSEMAIL);

                var buffer = System.Text.Encoding.UTF8.GetBytes(emailObject);
                ByteArrayContent byteContent = new(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resultt = client.PostAsync(Environment.GetEnvironmentVariable(GlobalConstantHelpers.URL_EMAIL), byteContent).Result;

                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS, GlobalConstantHelpers.METHODSENDEMAIL, LogTypeEnum.Information
                , $"{GlobalConstantMessages.LOGENDSENDEMAIL} {resultt.StatusCode}", GlobalConstantHelpers.NAMECLASSEMAIL);
                return true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS, GlobalConstantHelpers.METHODSENDEMAIL, LogTypeEnum.Information
                , $"{GlobalConstantMessages.LOGERRORSENDEMAIL} {ex.GetException()}", GlobalConstantHelpers.NAMECLASSEMAIL);
                return false;
            }
        }
    }
}

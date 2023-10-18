#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-28.
// Comment: Class that implements interface IEmail
//===============================================================================
#endregion
using Moq;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Common.Email
{
    public class EmailTest
    {
        private Mock<ILogService> logService;

        public EmailTest()
        {
            logService = new Mock<ILogService>();
        }
        [Theory]
        [InlineData("TokenTest")]
        public void EmailSendEmail_InputEmailDtoToken_ReturnsBolean(string token)
        {
            //Arrange
            List<string> listOfEmails = new List<string>();
            listOfEmails.Add("test@test.com");
            EmailDto emailDto = new()
            {
                TemplateId = 1,
                ReceiverEmails = listOfEmails.ToArray(),
                Parameters = new SendEmailParametersDto
                {
                    Name = "Test Test Test",
                    Password = "PassTest",
                    Date = DateTime.UtcNow,
                }
            };
            //Act
            var email = new Email(logService.Object);
            var result = email.SendEmail(emailDto, token);

            //Assert
            Assert.NotNull(result);
        }
    }
}

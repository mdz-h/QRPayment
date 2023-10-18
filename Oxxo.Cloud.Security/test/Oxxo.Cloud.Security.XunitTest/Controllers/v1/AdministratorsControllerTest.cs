#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-22.
// Comment: Class for unit test of AdministratorsController.cs
//===============================================================================
#endregion
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.WebUI.Controllers.v1;
using System.Net;
using System.Security.Claims;

namespace Oxxo.Cloud.Security.XunitTest.Controllers.v1
{
    public class AdministratorsControllerTest
    {
        readonly Mock<IMediator> mediator;
        readonly Mock<ILogService> log;

        public AdministratorsControllerTest()
        {
            mediator = new Mock<IMediator>();
            log = new Mock<ILogService>();
        }

        [Theory]
        [InlineData("3D30156B-2446-4455-EBD7-08DAE3C87C15", "-s!G:3166Y", "1s!G:3166Y",
           "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78")]
        public void AdministratorsControllerChangePassword_InputFromBody_ReturnNotNullAndHttpStatusCodeOK(string UserId, string OldPassword, string NewPassword, string token)
        {
            /// Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Authorization = token;
            var userIdentity = new ClaimsIdentity(new List<Claim>() { new Claim("userIdentification", "User"), new Claim("identification", "8B32FA3D-3A48-4B91-8325-1D14F73A6398") });
            httpContext.User = new ClaimsPrincipal(userIdentity);
            var administratorsController = new AdministratorsController(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext } };

            ChangePasswordCommand changePasswordCommand = new ChangePasswordCommand()
            {
                UserId = UserId,
                OldPassword = OldPassword,
                NewPassword = NewPassword
            };
            /// Act
            var response = administratorsController.PostChangePassword(changePasswordCommand);

            /// Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.IsCompleted);
            Assert.Equal(((OkResult)response.Result).StatusCode, (int)HttpStatusCode.OK);
        }
    }
}

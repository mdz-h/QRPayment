#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristáin (NEORIS).
// Date:    08/05/2023.
// Comment: PermissionsControllerTest
//===============================================================================
#endregion

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.WebUI.Controllers.v1;
using System.Net;
using System.Security.Claims;

namespace Oxxo.Cloud.Security.XunitTest.Controllers.v1
{
    public class PermissionsControllerTest
    {
        readonly Mock<IMediator> mediator;
        readonly Mock<ILogService> log;

        public PermissionsControllerTest()
        {
            this.mediator = new Mock<IMediator>();
            this.log = new Mock<ILogService>();
        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWlPWFhPIE5ldyIsInN1YiI6Im1pT1hYTyIsIklkIjoiNTkyMzFhM2YtMTU1Yy00MDY0LTgyOTMtZmYyOTk3MzVhNmU0IiwiZXhwIjoxNzM4NzUwMDAyLCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.1zTJWjip02yxPrIRJCz8P7ZadeZRm7YDev5-T22BaOY"
            , "OPERATING_INVENTORY", 0)]
        public void DMenuByRole_InputRequest_ReturnNotNullAndHttpStatusCodeOK(string token, string moduleName, int parentId)
        {            
            /// Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Authorization = token;
            var userIdentity = new ClaimsIdentity(new List<Claim>() { new Claim("userIdentification", "User"), new Claim("identification", "9EECAC09-FD19-428A-B98B-B9922D8A3EFA") });
            httpContext.User = new ClaimsPrincipal(userIdentity);
            var permissionsController = new PermissionsController(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext } };

            /// Act
            var response = permissionsController.MenuPerModule(moduleName, parentId);

            /// Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.IsCompleted);
            Assert.Equal(((OkObjectResult)response.Result).StatusCode, (int)HttpStatusCode.OK);
        }

    }
}

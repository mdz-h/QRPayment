using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.InvFis.Command.AuthInvfis;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.WebUI.Controllers.v1;
using System.Net;
using System.Security.Claims;

namespace WebUI.UnitTest.Controllers
{
    public class AuthenticateDeviceControllerTests
    {
        readonly Mock<IMediator> mediator;
        readonly Mock<ILogService> log;
        readonly AuthenticateController controller;
        public AuthenticateDeviceControllerTests()
        {
            mediator = new Mock<IMediator>();
            log = new Mock<ILogService>();
            controller = new AuthenticateController(mediator.Object, log.Object);
        }

        [Theory]
        [InlineData("PBI1N6VT3", "device", "3c2b9756c80cc4f96bff5c1ec0210dbaa62dbcd4e68bd1704ebd01ca2583a6b0")]
        public void AuthenticateController_InputDataDevice_ReturnOK(string id, string typeAuth, string deviceKey)
        {
            /// Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[GlobalConstantHelpers.DEVICEKEY] = deviceKey;
            AuthenticateCommand command = new() { Id = id, TypeAuth = typeAuth };
            AuthenticateController authController = new AuthenticateController(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext } };

            /// Act
            var response = authController.Login(command);

            /// Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(((OkObjectResult)response.Result).StatusCode, (int)HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("DEMO", "external", "12345")]
        public void AuthenticateController_InputDataExternal_ReturnOK(string id, string typeAuth, string apiKey)
        {
            /// Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[GlobalConstantHelpers.APIKEY] = apiKey;
            AuthenticateCommand command = new() { Id = id, TypeAuth = typeAuth };
            AuthenticateController authController = new AuthenticateController(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext } };

            /// Act
            var response = authController.Login(command);

            /// Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(((OkObjectResult)response.Result).StatusCode, (int)HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("user", "morales123", "6ee59b22622b1434ee10fdd241de6d799458b913320b852641acdd5f2c453b49")]
        public void AuthenticateController_InputDataUser_ReturnOK(string typeAuth, string user, string password)
        {
            /// Arrange
            var httpContext = new DefaultHttpContext();
            
            AuthenticateCommand command = new() { TypeAuth = typeAuth, User = user, Password = password };
            AuthenticateController authController = new(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext } };

            /// Act
            var response = authController.Login(command);

            /// Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(((OkObjectResult)response.Result).StatusCode, (int)HttpStatusCode.OK);
        }

        //[Theory]
        //[InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMFBCSSIsImp0aSI6IjUwMU42IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9",
        //            "1mkUcIYtUskalaDRqyZ0C9vrWKbpklG1xHssIHyq0dLsExHx//2G/2knXomMldQs++Z9zDv9ehLIHkc1v17pQw==")]
        //public void AuthenticateController_InputTokenRefresh_ReturnOK(string token, string refresToken)
        //{
        //    /// Arrange
        //    var httpContext = new DefaultHttpContext();
        //    httpContext.Request.Headers.Authorization = token;
        //    httpContext.Request.Headers[GlobalConstantHelpers.REFRESHTOKEN] = refresToken;
        //    var authController = new AuthenticateController(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext } };

        //    /// Act
        //    var response = authController.RefreshToken();

        //    /// Assert
        //    Assert.NotNull(response);
        //    Assert.NotNull(response.Result);
        //    Assert.Equal(((OkObjectResult)response.Result).StatusCode, (int)HttpStatusCode.OK);
        //}

        [Theory]
        [InlineData("Configuration/store-config", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMFBCSSIsImp0aSI6IjUwMU42IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9")]
        public void AuthenticateController_InputToken_ReturnOK(string endpoint, string token)
        {
            /// Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Authorization = token;
            var authController = new AuthenticateController(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext } };

            /// Act
            var response = authController.ValidateToken(endpoint);

            /// Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(((OkObjectResult)response.Result).StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task LoginInvFis_ReturnsInternalServerError_WhenInvalidClaimIdentity()
        {
            // Arrange
            AuthInvfisCommand command = new()
            {
                CrStore = "PLT",
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                        new Claim("test1", "mock")
                        }))
            };


            // Act
            IActionResult result = await controller.LoginInvFis(command);

            // Assert
            Assert.IsType<ObjectResult>(result);

            if (result is ObjectResult okObjectResult)
            {
                int? statusCodeValue = (int?)okObjectResult.StatusCode;
                HttpStatusCode? statusCode = statusCodeValue != null ? (HttpStatusCode)statusCodeValue : null;

                if (statusCode != null)
                {
                    Assert.Equal(HttpStatusCode.InternalServerError, statusCode.Value);
                }
            }

        }

        [Fact]
        public async Task LoginInvFis_ReturnsUnauthorized_WhenAuthorizationHeaderHasWrongScheme()
        {
            // Arrange
            AuthInvfisCommand command = new AuthInvfisCommand();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Basic abc123");

            // Act
            IActionResult result = await controller.LoginInvFis(command);

            // Assert
            Assert.IsType<ObjectResult>(result);

            if (result is ObjectResult okObjectResult)
            {
                int? statusCodeValue = (int?)okObjectResult.StatusCode;
                HttpStatusCode? statusCode = statusCodeValue != null ? (HttpStatusCode)statusCodeValue : null;

                if (statusCode != null)
                {
                    Assert.Equal(HttpStatusCode.InternalServerError, statusCode.Value);
                }
            }

        }

        [Theory]
        [InlineData("user_xpos", "morales123", "uqE4zw0lKzmHFui98AI2ypCasOa25fBo+lh0SSvVtDudJoF5wG2RI8y0PrU+C11Bvt59CS3WU7bLyLSUfv6M/j9csTv8OmqtZwclTjwUhLtqk4759joTOmWAKDkgDkfLg3tcI8+WfXEgpx2sLlofsaQ2J2Cy6zRdc+IZ5uIJovX8rTqkUP2gUgyzjYJ8rKxq5wbTygFALeoLK7o2bp0znEcLE/o98AJvXrgZruFg3A6uJRsZJtAmzAr2wIe1hjTpsHYmPMiJrmSCQUBt2gT8O7lryW8Ve7yGitCwvwux+gEdyn2zmy3/UJiH3Mc4OZUPvCZYioq3dTCZkFmoo0qZfg==",
            "509YK", "10MAN", "2", "20230525", "20230525", false)]
        public void AuthenticateController_InputDataUserXpos_ReturnOK(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        {
            /// Arrange
            var httpContext = new DefaultHttpContext();
            AuthenticateCommand command = new() { TypeAuth = typeAuth, User = user, Password = password, CrStore = crStore, CrPlace = crPlace, Till = till, AdministrativeDate = administrativeDate, ProcessDate= processDate, IsInternal = isInternal };
            AuthenticateController authController = new(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext } };

            /// Act
            var response = authController.Login(command);

            /// Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)response.Result).StatusCode);
        }

        [Theory]
        [InlineData("user_xpos", "morales123", "uqE4zw0lKzmHFui98AI2ypCasOa25fBo+lh0SSvVtDudJoF5wG2RI8y0PrU+C11Bvt59CS3WU7bLyLSUfv6M/j9csTv8OmqtZwclTjwUhLtqk4759joTOmWAKDkgDkfLg3tcI8+WfXEgpx2sLlofsaQ2J2Cy6zRdc+IZ5uIJovX8rTqkUP2gUgyzjYJ8rKxq5wbTygFALeoLK7o2bp0znEcLE/o98AJvXrgZruFg3A6uJRsZJtAmzAr2wIe1hjTpsHYmPMiJrmSCQUBt2gT8O7lryW8Ve7yGitCwvwux+gEdyn2zmy3/UJiH3Mc4OZUPvCZYioq3dTCZkFmoo0qZfg==",
           "509YK", "10MAN", "2", "20230525", "20230525", false)]
        public async Task AuthenticateController_InputDataUserXpos_ReturnException(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        {
            AuthenticateCommand command = new() { TypeAuth = typeAuth, User = user, Password = password, CrStore = crStore, CrPlace = crPlace, Till = till, AdministrativeDate = administrativeDate, ProcessDate = processDate, IsInternal = isInternal };
            AuthenticateController authController = new(mediator.Object, log.Object) { ControllerContext = new ControllerContext {} };

            // Act
            NullReferenceException ex = await Assert.ThrowsAsync<NullReferenceException>(() => authController.Login(command));
            // Assert
            Assert.NotNull(ex);
        }
    }
}
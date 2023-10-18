#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-11-26.
// Comment: Class for unit test of DevicesController.cs
//===============================================================================
#endregion
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Device.Commands.Register;
using Oxxo.Cloud.Security.WebUI.Controllers.v1;
using System.Net;
using System.Security.Claims;

namespace Oxxo.Cloud.Security.XunitTest.Controllers.v1
{
    public class DevicesControllerTest
    {
        readonly Mock<IMediator> mediator;
        readonly Mock<ILogService> log;

        public DevicesControllerTest()
        {
            mediator = new Mock<IMediator>();
            log = new Mock<ILogService>();
        }

        [Theory]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "10MAN", "509YK", 1, "44:8A:5B:37:F1:64", "10.200.10.223",
            "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKOMAR", "10MAN", "50D49", 1, "55:4A:2B:37:F3:56", "10.106.11.226",
            "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "10MAN", "507VH", 3, "99:4A:2B:37:F3:66", "10.106.11.226",
            "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public void DevicesControllerRegister_InputDataDevice_ReturnNotNullAndHttpStatusCodeOK(string deviceIdentifier, string deviceName, string crPlace, string crStore,
            int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            /// Arrange
            var devicesController = new DevicesController(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };
            RegisterDeviceCommand registerDeviceCommand = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
                DeviceName = deviceName,
                CrPlace = crPlace,
                CrStore = crStore,
                TillNumber = tillNumber,
                MacAddress = macAddress,
                IP = ip,
                Processor = processor,
                NetworkCard = networkCard,
                IsServer = isServer
            };

            /// Act
            var response = devicesController.Register(registerDeviceCommand);

            /// Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.IsCompleted);
            Assert.Equal(((StatusCodeResult)response.Result).StatusCode, (int)HttpStatusCode.Created);
        }

        [Theory]
        [InlineData(10, 1, "F1E12D08-EC6C-ED11-ADE5-A04A5E6D758C",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78")]
        [InlineData(10, 1, "", "")]
        [InlineData(-10, -1, "", "")]
        [InlineData(null, null, null, null)]
        public void DevicesControllerGET_InputFromQuery_ReturnNotNullAndHttpStatusCodeOK(int itemsNumber, int pageNumber, string deviceIdentifier, string token)
        {
            /// Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Authorization = token;
            var userIdentity = new ClaimsIdentity(new List<Claim>() { new Claim("userIdentification", "User"), new Claim("identification", "8B32FA3D-3A48-4B91-8325-1D14F73A6398") });
            httpContext.User = new ClaimsPrincipal(userIdentity);
            var devicesController = new DevicesController(mediator.Object, log.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext } };

            /// Act
            var response = devicesController.Get(itemsNumber, pageNumber, deviceIdentifier);

            /// Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.IsCompleted);
            Assert.Equal(((OkObjectResult)response.Result).StatusCode, (int)HttpStatusCode.OK);
        }
    }
}

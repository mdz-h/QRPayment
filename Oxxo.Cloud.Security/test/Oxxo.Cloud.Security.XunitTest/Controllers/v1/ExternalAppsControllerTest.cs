#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-14.
// Comment: Class Test of External Apps Controller.
//===============================================================================
#endregion

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.WebUI.Controllers.v1;

namespace Oxxo.Cloud.Security.XunitTest.Controllers.v1
{
    public class ExternalAppsControllerTest
    {
        #region Properties
        /// <summary>
        /// Contract Mock of Mediator
        /// </summary>
        readonly Mock<IMediator> _mediator;

        /// <summary>
        /// Contract Mock of LogService
        /// </summary>
        readonly Mock<ILogService> _log;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ExternalAppsControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _log = new Mock<ILogService>();
        }
        #endregion
        #region Test Methods
        [Theory]
        [InlineData("1234", 10, 1)]
        [InlineData("", 0, 0)]
        [InlineData(null, null, null)]
        public void GetShould(string identifier, int itemsNumber, int pagenumber)
        {
            // Arrange            
            var controller = new ExternalAppsController
                (_mediator.Object, _log.Object)
            { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };

            // Act
            var response = controller.Get(itemsNumber, pagenumber, identifier);

            // Assert
            Assert.NotNull(response);
        }

        /// <summary>
        /// Invoke controller
        /// </summary>
        [Fact]
        public void Generate()
        {
            // Arrange            
            var controller = new ExternalAppsController
                (_mediator.Object, _log.Object)
            { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };

            // Act
            var response = controller
                    .GenerateKey(new Application.ExternalApps.Commands.GenerateApiKey.GenerateApiKeyCommand() { ExternalAppId = "XXXX-XXXX-YYY" });

            // Assert
            Assert.NotNull(response);
        }

        /// <summary>
        /// Invoke controller
        /// </summary>
        [Fact]
        public void DeleteApiKey()
        {
            // Arrange            
            var controller = new ExternalAppsController
                (_mediator.Object, _log.Object)
            { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };

            // Act
            var response = controller
                    .DeleteKey(new Application.ExternalApps.Commands.DeleteApiKey.DeleteApiKeyCommand() { ExternalAppId = "XXXX-XXXX-YYY", APIKey = "OXXOCLOUDPRSWXQgWszwzOCays8dadLGsKnL6WkYZ2FQDnOtBKDXiC7TdPTVP6CFjwRoYlCWGOJocAu5lmE973Drf1xLeI4udJhJH2Nodtqs0s0RzdIZUTAZ9N1zic1e" });

            // Assert
            Assert.NotNull(response);
        }

        /// <summary>
        /// Invoke controller
        /// </summary>
        [Fact]
        public void AssignExternalAppsToRole()
        {
            // Arrange            
            var controller = new ExternalAppsController
                (_mediator.Object, _log.Object)
            { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };

            // Act
            var response = controller
                    .AssignExternalAppsToRole(new Application.ExternalApps.Commands.AssignExternalAppsToRole.AssignExternalAppsToRoleCommand() { ExternalAppId = "XXXX-XXXX-YYY", WorkgroupId = 2 });

            // Assert
            Assert.NotNull(response);
        }

        #endregion
    }
}
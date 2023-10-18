#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-12.
// Comment: Class Test of Create External Apps.
//===============================================================================
#endregion

using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.CreateExternalApps
{
    public class CreateExternalAppsCommandTest
    {
        /// <summary>
        /// Contract of ApplicationDbContext
        /// </summary>
        private readonly Mock<IApplicationDbContext> _context;

        /// <summary>
        /// Contract of LogService
        /// </summary>
        private readonly Mock<ILogService> _logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public CreateExternalAppsCommandTest()
        {
            _context = new Mock<IApplicationDbContext>();
            _logService = new Mock<ILogService>();
        }

        /// <summary>
        /// Test of Handle
        /// </summary>
        /// <param name="Name">Name of application</param>
        /// <param name="code">Code of application</param>
        [Theory]
        [InlineData("APPTEST", "Aplication test")]
        public void CreatePermissionsCommand_ValidateHandle_ReturnBool(string code, string name)
        {
            // Arrange
            CreateExternalAppsCommand command = new()
            {
                Code = code,
                Name = name
            };
            CreateExternalAppsCommandHandle handler = new CreateExternalAppsCommandHandle(_context.Object, _logService.Object);

            // Act
            Task result = handler.Handle(command, new CancellationToken());

            // Asert
            Assert.NotNull(result);
        }
    }
}
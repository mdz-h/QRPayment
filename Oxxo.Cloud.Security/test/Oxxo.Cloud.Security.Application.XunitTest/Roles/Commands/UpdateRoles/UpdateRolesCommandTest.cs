using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.UpdateRoles
{
    public class UpdateRolesCommandTest
    {
        private Mock<IApplicationDbContext> context;
        private Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public UpdateRolesCommandTest()
        {
            this.context = new Mock<IApplicationDbContext>(); ;
            this.logService = new Mock<ILogService>(); ;
        }

        /// <summary>
        /// validate if the class handler not return null
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="code"></param>
        /// <param name="description"></param>        
        [Theory]
        [InlineData(7, "DEVICE_TILL4", true, "DEVICE_TILL5D")]
        [InlineData(4, "DEVICE_TILL", false, "DEVICE_TILL")]
        public void UpdateRolesCommand_ValidateHandle_ReturnBool(int roleId, string shortName, bool isActive, string description)
        {
            UpdateRolesCommand command = new()
            {
                RoleId = roleId,
                ShortName = shortName,
                IsActive = isActive,
                Description = description
            };
            var handler = new UpdateRolesHandler(context.Object, logService.Object);
            //Act
            var result = handler.Handle(command, new CancellationToken());
            //Asert
            Assert.NotNull(result);
        }
    }
}

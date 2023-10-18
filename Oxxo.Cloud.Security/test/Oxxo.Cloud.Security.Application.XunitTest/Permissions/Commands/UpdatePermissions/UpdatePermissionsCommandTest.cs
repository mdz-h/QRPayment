using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.UpdatePermissions
{
    public class UpdatePermissionsCommandTest
    {
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public UpdatePermissionsCommandTest()
        {
            this.context = new Mock<IApplicationDbContext>();
            this.logService = new Mock<ILogService>();
        }

        /// <summary>
        /// validate if the class handler not return null
        /// </summary>
        /// <param name="permissionID">Permission ID</param>
        /// <param name="Name">Short name of permission</param>
        /// <param name="code">Code of permission</param>
        /// <param name="description">Description of permissio</param>
        /// <param name="moduleId"></param>
        /// <param name="permissionTypeID">permission type ID</param>
        [Theory]
        [InlineData(11, "123456", "12345", "12345", 1, 1)]
        public void UpdatePermissionsCommand_ValidateHandle_ReturnBool(int permissionID, string Name, string code, string description, int moduleId, int permissionTypeID)
        {

            UpdatePermissionsCommand command = new()
            {
                PermissionID = permissionID,
                Name = Name,
                Code = code,
                Description = description,
                ModuleID = moduleId,
                PermissionTypeID = permissionTypeID
            };

            var handler = new UpdatePermissionsHandler(logService.Object, context.Object);

            //Act
            var result = handler.Handle(command, new CancellationToken());

            //Asert
            Assert.NotNull(result);
        }
    }
}

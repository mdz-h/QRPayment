using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Permissions.Commands.DeletePermissions;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommandTest
    {
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public DeletePermissionCommandTest()
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
        [InlineData(11)]
        public void DeletePermissionsCommand_ValidateHandle_ReturnBool(int permissionID)
        {

            DeletePermissionsCommand command = new()
            {
                PermissionID = permissionID
            };

            var handler = new DeletePermissionsHandler(logService.Object, context.Object);

            //Act
            var result = handler.Handle(command, new CancellationToken());

            //Asert
            Assert.NotNull(result);
        }
    }
}

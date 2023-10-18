using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.CreatePermissions
{
    public class CreatePermissionsCommandTest
    {
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public CreatePermissionsCommandTest()
        {
            this.context = new Mock<IApplicationDbContext>();
            this.logService = new Mock<ILogService>();
        }

        /// <summary>
        /// validate if the class handler not return null
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <param name="moduleId"></param>
        /// <param name="permissionTypeID"></param>
        [Theory]
        [InlineData("123456", "12345", "12345", 1, 1)]
        public void CreatePermissionsCommand_ValidateHandle_ReturnBool(string Name, string code, string description, int moduleId, int permissionTypeID)
        {

            CreatePermissionsCommand command = new()
            {
                Name = Name,
                Code = code,
                Description = description,
                ModuleID = moduleId,
                PermissionTypeID = permissionTypeID
            };

            var handler = new CreatePermissionsHandler(logService.Object, context.Object);

            //Act
            var result = handler.Handle(command, new CancellationToken());

            //Asert
            Assert.NotNull(result);
        }
    }
}

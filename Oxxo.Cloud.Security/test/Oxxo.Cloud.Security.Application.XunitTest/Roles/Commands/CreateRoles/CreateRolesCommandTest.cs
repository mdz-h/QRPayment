using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.CreateRoles
{
    public class CreateRolesCommandTest
    {
        private Mock<IApplicationDbContext> context;
        private Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public CreateRolesCommandTest()
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
        [InlineData("DEVICE_TILL5", "DEVICE_TILL5", "DEVICE_TILL5D")]
        public void CreateRolesCommand_ValidateHandle_ReturnBool(string shortName, string code, string description)
        {
            CreateRolesCommand command = new()
            {
                ShortName = shortName,
                Code = code,
                Description = description
            };
            var handler = new CreateWorkgroupHandler(context.Object, logService.Object);
            //Act
            var result = handler.Handle(command, new CancellationToken());
            //Asert
            Assert.NotNull(result);
        }
    }
}

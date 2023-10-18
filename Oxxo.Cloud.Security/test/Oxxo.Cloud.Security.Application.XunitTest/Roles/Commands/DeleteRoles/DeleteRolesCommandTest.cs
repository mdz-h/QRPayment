using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.DeleteRoles
{
    public class DeleteRolesCommandTest
    {
        private Mock<IApplicationDbContext> context;
        private Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public DeleteRolesCommandTest()
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
        [InlineData(2)]
        [InlineData(3)]
        public void DeleteRolesCommand_ValidateHandle_ReturnBool(int roleId)
        {
            DeleteRolesCommand command = new()
            {
                RoleId = roleId
            };
            var handler = new DeleteRoleHandler(context.Object, logService.Object);
            //Act
            var result = handler.Handle(command, new CancellationToken());
            //Asert
            Assert.NotNull(result);
        }
    }
}

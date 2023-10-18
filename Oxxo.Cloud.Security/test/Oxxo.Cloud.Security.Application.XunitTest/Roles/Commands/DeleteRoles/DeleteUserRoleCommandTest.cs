using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.DeleteRoles
{
    public class DeleteUserRoleCommandTest
    {
        private Mock<IApplicationDbContext> context;
        private Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public DeleteUserRoleCommandTest()
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
        [InlineData("59231A3F-155C-4064-8293-FF299735A6E4")]
        [InlineData("875E56A7-C763-47F7-94F9-B0170882FA74")]
        public void DeleteUserRoleCommand_ValidateHandle_ReturnBool(string userId)
        {
            DeleteUserRoleCommand command = new()
            {
                UserId = userId
            };
            var handler = new DeleteUserRoleCommandHandler(context.Object, logService.Object);
            //Act
            var result = handler.Handle(command, new CancellationToken());
            //Asert
            Assert.NotNull(result);
        }

    }
}

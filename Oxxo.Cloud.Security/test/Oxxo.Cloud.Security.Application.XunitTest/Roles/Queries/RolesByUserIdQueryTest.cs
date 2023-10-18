using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using static Oxxo.Cloud.Security.Application.Roles.Queries.RolesByUserIdQuery;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesByUserIdQueryTest
    {
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<ILogService> logService;

        public RolesByUserIdQueryTest()
        {
            this.context = new Mock<IApplicationDbContext>();
            this.logService = new Mock<ILogService>();
        }
        /// <summary>
        /// validate if the class handler not return null
        /// </summary>
        /// <param name="roleId"></param>        
        [Theory]
        [InlineData("59231A3F-155C-4064-8293-FF299735A6E4")]
        [InlineData("875E56A7-C763-47F7-94F9-B0170882FA74")]
        public void GetRolesByUserIdQuery_ValidateHandle_ReturnBool(string userId)
        {
            RolesByUserIdQuery query = new()
            {
                UserId = userId
            };
            var handler = new RolesByUserIdQueryHandler(context.Object, logService.Object);
            //Act
            var result = handler.Handle(query, new CancellationToken());
            //Asert
            Assert.NotNull(result);
        }
    }
}

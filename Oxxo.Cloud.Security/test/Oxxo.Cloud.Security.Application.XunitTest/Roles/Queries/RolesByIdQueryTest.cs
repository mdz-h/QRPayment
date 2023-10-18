using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using static Oxxo.Cloud.Security.Application.Roles.Queries.RolesByIdQuery;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesByIdQueryTest
    {
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public RolesByIdQueryTest()
        {
            this.context = new Mock<IApplicationDbContext>();
            this.logService = new Mock<ILogService>();
        }

        /// <summary>
        /// validate if the class handler not return null
        /// </summary>
        /// <param name="roleId"></param>        
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetRolesByIdQuery_ValidateHandle_ReturnBool(int roleId)
        {
            RolesByIdQuery query = new()
            {
                RoleId = roleId
            };
            var handler = new RolesByIdQueryHandler(context.Object, logService.Object);
            //Act
            var result = handler.Handle(query, new CancellationToken());
            //Asert
            Assert.NotNull(result);
        }
    }
}

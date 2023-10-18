using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using static Oxxo.Cloud.Security.Application.Roles.Queries.RolesQuery;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesQueryTest
    {
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<ILogService> logService;

        public RolesQueryTest()
        {
            this.context = new Mock<IApplicationDbContext>();
            this.logService = new Mock<ILogService>();
        }

        /// <summary>
        /// validate if the class handler not return null
        /// </summary>
        /// <param name="roleId"></param>        
        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, 3)]
        [InlineData(2, 4)]
        public void GetRolesQuery_ValidateHandle_ReturnBool(int skip, int take)
        {
            RolesQuery query = new()
            {
                Skip = skip,
                Take = take
            };
            var handler = new RolesQueryHandler(context.Object, logService.Object);
            //Act
            var result = handler.Handle(query, new CancellationToken());
            //Asert
            Assert.NotNull(result);
        }
    }
}

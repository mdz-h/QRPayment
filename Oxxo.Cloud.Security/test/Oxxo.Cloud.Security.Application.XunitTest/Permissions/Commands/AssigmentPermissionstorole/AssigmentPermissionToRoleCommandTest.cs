#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-12-13.
// Comment: Command Assigment Permission to Role Test.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.AssigmentPermissionstorole
{
    public class AssigmentPermissionToRoleCommandTest
    {

        private readonly Mock<ILogService> logService;
        private
        readonly ApplicationDbContext context;
        readonly Mock<IMediator> Mediator;

        public AssigmentPermissionToRoleCommandTest()
        {

            logService = new Mock<ILogService>();
            Mediator = new Mock<IMediator>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            context = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor());
        }
        public static IEnumerable<object[]> RegistrerAssigmentroletoUserData()
        {
            yield return new object[] { 18, new List<int> { 10, 11, 15 } };
            yield return new object[] { 11, new List<int> { 40, 41, 42 } };
        }

        [Theory]
        [MemberData(nameof(RegistrerAssigmentroletoUserData))]
        public void RegistrerAssigmentroletoUser_ReturnNotNullIsCompletedTrue(int WorkgroupId, List<int> PermissionId)
        {
            //Arrange
            AssigmentPermissionToRoleCommand assigmentPermissionToRoleCommand = new AssigmentPermissionToRoleCommand()
            {
                WorkgroupId = WorkgroupId,
                PermissionId = PermissionId
            };
            //Act
            var getAssigmentRoltoUser = new AssigmentPermissionToRoleCommandHandler(context, logService.Object);
            var response = getAssigmentRoltoUser.Handle(assigmentPermissionToRoleCommand, new CancellationToken());

            //Assert

            Assert.NotNull(response);
        }


    }
}

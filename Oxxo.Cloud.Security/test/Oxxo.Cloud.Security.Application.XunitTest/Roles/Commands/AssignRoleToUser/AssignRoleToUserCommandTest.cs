#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-12-13.
// Comment: Command Assigment Roles to User Validation Test.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandTest
    {
        private readonly Mock<ILogService> logService;
        private
        readonly ApplicationDbContext context;
        readonly Mock<IMediator> Mediator;

        public AssignRoleToUserCommandTest()
        {

            logService = new Mock<ILogService>();
            Mediator = new Mock<IMediator>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            context = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor());
        }

        [Theory]
        [InlineData("6ab5a6a4-a9b6-40ac-0f00-08dadd1c5a2b", 5)]
        [InlineData("a283fbb9-5f7e-4d64-e3e6-08dadd1d9e4e", 5)]
        public void AssignRolestoUserInsertRoletouser(string GUID, int WORKGROUPID)
        {
            AssignRoleToUserCommand assignRoleToUserCommand = new AssignRoleToUserCommand()
            {
                Guid = GUID,
                WorkgroupId = WORKGROUPID
            };

            var cmd = new AssignRoleToUserCommandHandler(context, logService.Object);
            var response = cmd.Handle(assignRoleToUserCommand, new CancellationToken());

            Assert.NotNull(response);
        }

    }
}

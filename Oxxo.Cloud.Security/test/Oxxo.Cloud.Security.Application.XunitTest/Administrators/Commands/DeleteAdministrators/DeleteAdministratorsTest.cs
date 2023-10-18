#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
// Comment: Administrators.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.DeleteAdministrators
{
    /// <summary>
    /// Test class to validate administrators
    /// </summary>
    public class DeleteAdministratorsTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;

        /// <summary>
        /// Constructor Class
        /// </summary>
        public DeleteAdministratorsTest()
        {
            logService = new Mock<ILogService>();
            mediator = new Mock<IMediator>();

            List<Person> lstPerson = SetDataMock.AddPerson();
            List<OperatorStatus> lstOperatorStatus = SetDataMock.AddStatusData();
            List<Operator> lstOperator = SetDataMock.AddOperator();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.PERSON.AddRange(lstPerson);
            context.OPERATOR_STATUS.AddRange(lstOperatorStatus);
            context.OPERATOR.AddRange(lstOperator);
            context.SaveChanges();
        }

        /// <summary>
        /// Validate if the handler save changes
        /// </summary>
        /// <param name="username">user name</param>
        [Theory]
        [InlineData("Fredel")]
        public void Delete_Administrator_Ok_Test(string username)
        {
            var guid = context.OPERATOR.AsNoTracking().Where(w => w.USER_NAME == username).Select(s => s.OPERATOR_ID).FirstOrDefault();

            DeleteAdministratorsCommand admin = new DeleteAdministratorsCommand()
            {
                UserId = guid.ToString(),
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            var getHandler = new DeleteAdministratorsHandler(logService.Object, context);
            var response = getHandler.Handle(admin, new CancellationToken());

            Assert.True(response != null && response.IsCompleted && response.Result);
        }

        /// <summary>
        /// Validate if the handler not save changes
        /// </summary>
        /// <param name="username">user name</param>
        [Theory]
        [InlineData("Fredel001")]
        public async Task DeleteAdministratorNoExistsTest(string username)
        {
            var guid = context.OPERATOR.Where(w => w.USER_NAME == username).Select(s => s.OPERATOR_ID).FirstOrDefault();

            DeleteAdministratorsCommand admin = new()
            {
                UserId = guid.ToString()
            };

            var getHandler = new DeleteAdministratorsHandler(logService.Object, context);
            InvalidOperationException exception = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => getHandler.Handle(admin, new CancellationToken()));

            Assert.NotNull(exception);
        }
    }
}

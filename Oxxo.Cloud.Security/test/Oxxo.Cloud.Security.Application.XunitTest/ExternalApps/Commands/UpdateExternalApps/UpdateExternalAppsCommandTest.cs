using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.UpdateExternalApps
{
    public class UpdateExternalAppsCommandTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;

        public UpdateExternalAppsCommandTest()
        {
            logService = new Mock<ILogService>();
            mediator = new Mock<IMediator>();

            List<ExternalApplication> lstExternalApps = new List<ExternalApplication>();
            lstExternalApps.Add(new ExternalApplication()
            {
                EXTERNAL_APPLICATION_ID = new Guid("59231a3f-155c-4064-8293-ff299735a6e4"),
                CODE = "miOXXO",
                NAME = "miOXXO",
                TIME_EXPIRATION_TOKEN = 43800,
                IS_ACTIVE = true,
                LCOUNT = 1,
                CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                CREATED_DATETIME = DateTime.UtcNow,
                MODIFIED_BY_OPERATOR_ID = null,
                MODIFIED_DATETIME = null
            });

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.EXTERNAL_APPLICATION.AddRange(lstExternalApps);
            context.SaveChanges();
        }

        [Theory]
        [InlineData("59231a3f-155c-4064-8293-ff299735a6e4", "OTHER NAME TEST", true)]
        public void UpdateExernalAppUsingValidId(string externalAppId, string name, bool isActive)
        {
            UpdateExternalAppsCommand command = new UpdateExternalAppsCommand()
            {
                ExternalAppId = externalAppId,
                Name = name,
                IsActive = isActive,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            var handler = new UpdateExternalAppsHandler(context, logService.Object);
            var response = handler.Handle(command, new CancellationToken());

            Assert.True(response != null && response.IsCompleted && response.Result);
        }

        [Theory]
        [InlineData("", "OTHER NAME TEST", true)]
        [InlineData(null, "OTHER NAME TEST", true)]
        [InlineData("HELLO", "OTHER NAME TEST", true)]
        [InlineData("59231a3f-155c-4064-8293-bb299735a655", "OTHER NAME TEST", true)]
        public async Task UpdateNotFoundExernalApp(string externalAppId, string name, bool isActive)
        {
            UpdateExternalAppsCommand command = new()
            {
                ExternalAppId = externalAppId,
                Name = name,
                IsActive = isActive
            };
            var handler = new UpdateExternalAppsHandler(context, logService.Object);
            InvalidOperationException exception = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => handler.Handle(command, new CancellationToken()));

            Assert.NotNull(exception);
        }
    }
}

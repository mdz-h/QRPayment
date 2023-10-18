#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-26.
// Comment: DELETE API KEY.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.DeleteApiKey
{
    /// <summary>
    /// Unit Test to DeleteApiKeyCommandTest
    /// </summary>
    public class DeleteApiKeyCommandTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;


        /// <summary>
        /// Constructor class
        /// </summary>
        public DeleteApiKeyCommandTest()
        {
            logService = new Mock<ILogService>();
            mediator = new Mock<IMediator>();

            List<ExternalApplication> externalApps = SetDataMock.AddExternalApplication();
            List<ApiKey> apiKeys = SetDataMock.AddApiKeys();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
              .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.EXTERNAL_APPLICATION.AddRange(externalApps);
            context.API_KEY.AddRange(apiKeys);
            context.SaveChanges();
        }

        /// <summary>
        /// Validate if the method is valid
        /// </summary>
        [Fact]
        public void DeleteApiKey_Test()
        {
            DeleteApiKeyCommand command = new()
            {
                ExternalAppId = SetDataMock.GuidSessionToken.ToString(),
                APIKey = SetDataMock.Api_Key,
                Identification = SetDataMock.GuidSessionToken.ToString(),
            };

            var getHandler = new DeleteApiKeyHandler(logService.Object, context);
            var response = getHandler.Handle(command, new CancellationToken());

            Assert.True(response != null && response.IsCompleted && response.Result);
        }

        /// <summary>
        /// Validate if the method have record validation
        /// </summary>
        [Fact]
        public async Task DeleteApiKey_EmptyExternalAPP_Test()
        {
            DeleteApiKeyCommand command = new()
            {
                ExternalAppId = Guid.NewGuid().ToString(),
                APIKey = SetDataMock.Api_Key
            };

            var getHandler = new DeleteApiKeyHandler(logService.Object, context);

            InvalidOperationException exception = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => getHandler.Handle(command, new CancellationToken()));

            Assert.NotNull(exception);
        }
    }
}

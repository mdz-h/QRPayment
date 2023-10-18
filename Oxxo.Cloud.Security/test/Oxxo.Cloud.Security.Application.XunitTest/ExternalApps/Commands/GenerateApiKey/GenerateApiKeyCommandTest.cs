#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-22.
// Comment: GENERATE API KEY.
//===============================================================================
#endregion


using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.GenerateApiKey
{
    /// <summary>
    /// Unit Test GenerateApiKeyCommandTest
    /// </summary>
    public class GenerateApiKeyCommandTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<ICryptographyService> cripto;

        /// <summary>
        /// Constructor class
        /// </summary>
        public GenerateApiKeyCommandTest()
        {
            logService = new Mock<ILogService>();
            mediator = new Mock<IMediator>();
            cripto = new Mock<ICryptographyService>();

            List<SystemParam> lstSystemParam = SetDataMock.AddSystemParam();
            List<ExternalApplication> externalApps = SetDataMock.AddExternalApplication();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
              .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.SYSTEM_PARAM.AddRange(lstSystemParam);
            context.EXTERNAL_APPLICATION.AddRange(externalApps);
            context.SaveChanges();
        }

        /// <summary>
        /// This method validate if the API Key is created
        /// </summary>
        [Fact]
        public void Generate_ApiKey_ExternalAppCorrect()
        {
            GenerateApiKeyCommand command = new()
            {
                ExternalAppId = SetDataMock.GuidSessionToken.ToString(),
                Identification = SetDataMock.GuidSessionToken.ToString()
            };

            Common.Security.Security scurity = new Common.Security.Security(context);

            var getHandler = new GenerateApiKeyHandler(logService.Object, context, scurity, cripto.Object);
            var response = getHandler.Handle(command, new CancellationToken());

            Assert.True(response != null && response.IsCompleted && !string.IsNullOrWhiteSpace(response.Result.ApiKey));
        }

        /// <summary>
        /// This method validate if the API Key is generated
        /// </summary>
        [Fact]
        public async Task Generate_ApiKey()
        {
            var cancellationToken = new CancellationToken();

            Common.Security.Security scurity = new Common.Security.Security(context);
            List<SystemParam> systemParams = await this.context.SYSTEM_PARAM.ToListAsync(cancellationToken);
            var getMethod = new GenerateApiKeyHandler(logService.Object, context, scurity, cripto.Object);
            var response = getMethod.GetApiKey(systemParams, cancellationToken);

            Assert.True(response != null && response.IsCompleted && !string.IsNullOrWhiteSpace(response.Result));
        }
    }
}

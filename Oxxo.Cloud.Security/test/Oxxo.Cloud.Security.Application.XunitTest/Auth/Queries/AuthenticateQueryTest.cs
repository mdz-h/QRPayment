#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2023-01-03.
// Comment: Add related ApiKey.
//===============================================================================
#endregion


using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Auth.Queries
{
    /// <summary>
    /// Unit test to validate methods in AuthenticateQuery
    /// </summary>
    public class AuthenticateQueryTest
    {
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<ICryptographyService> cripto;

        /// <summary>
        /// Initial configurations in constructor class
        /// </summary>
        public AuthenticateQueryTest()
        {
            mediator = new Mock<IMediator>();
            cripto = new Mock<ICryptographyService>();


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
        /// Validate if the test return value
        /// </summary>
        /// <returns>API Key</returns>
        [Fact]
        public async Task GetApiKeyExternalApplication_Test()
        {
            AuthenticateQuery authenticateQuery = new(context, cripto.Object);
            string apiKey = await authenticateQuery.GetApiKeyExternalApplication("RP", SetDataMock.Api_Key_Decrypt) ?? string.Empty;

            Assert.NotNull(apiKey);
        }

        /// <summary>
        /// Validate if the test return empty value
        /// </summary>
        /// <returns>Empty value</returns>
        [Theory]
        [InlineData("RP", "BADAPIKEY")]
        [InlineData("BADCODE", SetDataMock.Api_Key)]
        public async Task GetApiKeyExternalApplication_Empty_Test(string code, string apiKey)
        {
            AuthenticateQuery authenticateQuery = new(context, cripto.Object);
            string apiKeyreturn = await authenticateQuery.GetApiKeyExternalApplication(code, apiKey) ?? string.Empty;

            Assert.Empty(apiKeyreturn);
        }
    }
}

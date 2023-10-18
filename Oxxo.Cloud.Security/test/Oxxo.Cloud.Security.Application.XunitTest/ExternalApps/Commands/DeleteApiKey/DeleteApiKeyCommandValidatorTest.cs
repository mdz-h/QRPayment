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
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.DeleteApiKey
{
    /// <summary>
    /// Unit test to validate class
    /// </summary>
    public class DeleteApiKeyCommandValidatorTest
    {
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;

        /// <summary>
        /// Constructor Class
        /// </summary>
        public DeleteApiKeyCommandValidatorTest()
        {
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
        /// This test validate if the fields are correct
        /// </summary>
        /// </summary>
        /// <param name="field">field name to validate</param>
        /// <param name="ExternalAppsId">External application Id</param>
        /// <param name="ApiKey">Key</param>
        /// <param name="msgError">Message Error</param>
        [Theory]
        [InlineData("External App Id", "", "OXXOCLOUDPRSWXQgWsz", GlobalConstantMessages.FIELDEMPTY, "400")]
        [InlineData("Api Key", "XXXX-YYYYY.XYXYXYXY", "", GlobalConstantMessages.FIELDEMPTY, "400")]
        public async Task DeleteApiKeyCommandValidator_Test(string field, string ExternalAppsId, string ApiKey, string msgError, string statusCode)
        {
            DeleteApiKeyCommand command = new()
            {
                ExternalAppId = ExternalAppsId,
                APIKey = ApiKey
            };

            msgError = string.Format(msgError, field).ToUpper();

            DeleteApiKeyCommandValidator commandValidator = new(context);
            var commandResponse = await commandValidator.ValidateAsync(command);

            bool isValid = commandResponse.Errors.Any(a => a.ToString().Trim().ToUpper() == msgError.ToUpper()) && commandResponse.Errors[0].ErrorCode == statusCode;


            Assert.True(isValid);
        }

        /// <summary>
        /// This test validate if the record is not inactive
        /// </summary>
        /// </summary>
        [Fact]
        public async Task DeleteApiKeyCommandValidatorInactive_Test()
        {
            DeleteApiKeyCommand command = new()
            {
                ExternalAppId = SetDataMock.GuidSessionToken.ToString(),
                APIKey = SetDataMock.Api_Key
            };


            List<ApiKey> apiKeys = await context.API_KEY.ToListAsync();

            apiKeys.Select(s => { s.IS_ACTIVE = false; return s; }).ToList();
            await context.SaveChangesAsync(new CancellationToken());

            DeleteApiKeyCommandValidator commandValidator = new(context);
            var commandResponse = await commandValidator.ValidateDeleted(command, new CancellationToken());

            Assert.True(!commandResponse);
        }

        /// <summary>
        /// This test validate if the record is not inactive
        /// </summary>
        /// </summary>
        [Fact]
        public async Task DeleteApiKeyCommandValidatorExists_Test()
        {
            DeleteApiKeyCommand command = new()
            {
                ExternalAppId = "XXXXX-YYYYY-X",
                APIKey = "BADAPIKEY"
            };

            List<ApiKey> apiKeys = await context.API_KEY.ToListAsync();

            apiKeys.Select(s => { s.IS_ACTIVE = false; return s; }).ToList();
            await context.SaveChangesAsync(new CancellationToken());

            DeleteApiKeyCommandValidator commandValidator = new(context);
            var commandResponse = await commandValidator.ValidateExistsApiKey(command, new CancellationToken());

            Assert.True(!commandResponse);
        }
    }
}

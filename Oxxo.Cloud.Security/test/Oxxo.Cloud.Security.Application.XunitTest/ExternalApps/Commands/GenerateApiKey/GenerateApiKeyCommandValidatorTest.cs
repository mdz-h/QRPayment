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
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.GenerateApiKey
{
    public class GenerateApiKeyCommandValidatorTest
    {
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;

        /// <summary>
        /// Constructor class
        /// </summary>
        public GenerateApiKeyCommandValidatorTest()
        {
            mediator = new Mock<IMediator>();

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
        /// This test validate if the fields are correct
        /// </summary>
        /// <param name="fieldname">field name to validate</param>
        /// <param name="ExternalAppId">external app id</param>
        /// <param name="msgError">Message error</param>
        /// <param name="statusCode">Error Status Code</param>
        /// <returns></returns>
        [Theory]
        [InlineData("external App id", null, GlobalConstantMessages.FIELDEMPTY, "400")]
        [InlineData("", "XXXXXXXXX-XXXXXXXX-YYYYYYYY-YYYYYYY", GlobalConstantMessages.NOTRECORD, "422")]
        public async Task Generate_ApiKey_EmptyValues(string fieldname, string ExternalAppId, string msgError, string statusCode)
        {
            GenerateApiKeyCommand command = new()
            {
                ExternalAppId = ExternalAppId
            };

            GenerateApiKeyCommandValidator commandValidator = new(context);
            var commandResponse = await commandValidator.ValidateAsync(command);

            msgError = string.Format(msgError, fieldname);

            bool isValid = commandResponse.Errors.Any(a => a.ToString().Trim().ToUpper() == msgError.ToUpper()) && commandResponse.Errors[0].ErrorCode == statusCode;
            Assert.True(isValid);
        }
    }
}

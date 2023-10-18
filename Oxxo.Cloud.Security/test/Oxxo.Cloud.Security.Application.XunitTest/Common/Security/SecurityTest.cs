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
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Common.Security
{
    /// <summary>
    /// Unit Test Security Method
    /// </summary>
    public class SecurityTest
    {
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;


        /// <summary>
        /// Constructor Class
        /// </summary>
        public SecurityTest()
        {
            mediator = new Mock<IMediator>();

            List<SystemParam> lstSystemParam = SetDataMock.AddSystemParam();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
             .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.SYSTEM_PARAM.AddRange(lstSystemParam);
            context.SaveChanges();
        }

        /// <summary>
        /// Validate if the method return data
        /// </summary>
        [Fact]
        public async Task GetSystemParamsApiKeyRules_Get_Values()
        {
            Security objsecurity = new(context);

            List<SystemParam> systemParams = await objsecurity.GetSystemParamsApiKeyRules(new CancellationToken());

            Assert.True(systemParams.Any());
        }

        /// <summary>
        ///  Validate System parameters configured
        /// </summary> 
        /// <param name="paramcode">System Parameters Code</param>
        /// <param name="msgError">Message error</param>
        [Theory]
        [InlineData(GlobalConstantHelpers.APIKEYPREFIX, GlobalConstantMessages.CONFIGSYSTEMPARAMNOTFOUND)]
        [InlineData(GlobalConstantHelpers.APIKEYSECUREBYTES, GlobalConstantMessages.CONFIGSYSTEMPARAMNOTFOUND)]
        [InlineData(GlobalConstantHelpers.APIKEYEXPIRATIONTIME, GlobalConstantMessages.CONFIGSYSTEMPARAMNOTFOUND)]
        [InlineData("", GlobalConstantMessages.CONFIGSYSTEMPARAMAPIKEYSRULES)]

        public void ValidateApiKeyRules_Test(string paramcode, string msgError)
        {
            List<SystemParam> lstParams = string.IsNullOrWhiteSpace(paramcode) ? new List<SystemParam>() : SetDataMock.AddSystemParam();

            lstParams = lstParams.Where(w => w.PARAM_CODE != paramcode).ToList();

            Security objsecurity = new(context);

            CustomException exception = Assert.Throws<CustomException>(() => objsecurity.ValidateApiKeyRules(lstParams));

            msgError = string.Format(msgError, paramcode);

            Assert.Equal(msgError, exception.Message);
        }

        /// <summary>
        /// Validate if the API Key is generate
        /// </summary>
        [Fact]
        public void GenerateApiKey_Test()
        {
            List<SystemParam> lstParams = SetDataMock.AddSystemParam();
            Security objsecurity = new(context);

            string apiKey = objsecurity.GenerateApiKey(lstParams);

            Assert.NotEmpty(apiKey);
        }
    }
}

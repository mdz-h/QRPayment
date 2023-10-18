#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    13/12/2022.
// Comment: Administrators.
//===============================================================================
#endregion 

using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;
using Oxxo.Cloud.Security.Infrastructure.Services;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.CreateAdministrators
{
    /// <summary>
    /// Test class to validate administrators
    /// </summary>
    public class CreateAdministratorTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IEmail> iemail;
        private readonly CryptographyService cryptographyService;
        private readonly string publicKey = "YfDERuiEZsEy3qCmEbksfZeLYGkkCYvDLqtLnqjxAE4i90uG/+YUaRvrdOWdQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private readonly string privateKey = "bPmDYzJxY9LR/3E8O9FKkxBRN8UXIVXA6EXYf8W5lNYD6Kpfgl5Q/pel/WPeNrK84u/ZD7Swo8E2HqDWykz/N5rBEbF00xFt8WXGyPLKV214B9EZ";

        /// <summary>
        /// Constructor Class
        /// </summary>
        public CreateAdministratorTest()
        {
            logService = new Mock<ILogService>();
            mediator = new Mock<IMediator>();
            iemail = new Mock<IEmail>();
            cryptographyService = new CryptographyService(publicKey ?? string.Empty, privateKey ?? string.Empty);
            List<SystemParam> lstSystemParam = SetDataMock.AddSystemParam();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
              .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.SYSTEM_PARAM.AddRange(lstSystemParam);
            context.SaveChanges();
        }

        /// <summary>
        ///  Validate if the handler save changes
        /// </summary>
        /// <param name="nickname">Nick Name</param>
        /// <param name="username">User Name</param>
        /// <param name="middlename">Middle Name</param>
        /// <param name="lastnamepat">Last name paternal</param>
        /// <param name="lastnamemat">last name maternal</param>
        /// <param name="email">email address</param>
        [Theory]
        [InlineData("OxxoCloud", "Oxxo", "MID", "México", "México", "oxxo.cloud@oxxo.com")]
        public void Create_Administrator_Ok_Test(string userName, string name, string middlename, string lastnamepat, string lastnamemat, string email)
        {
            CreateAdministratorCommand command = new()
            {
                Name = name,
                UserName = userName,
                LastNamePat = lastnamepat,
                LastNameMat = lastnamemat,
                MiddleName = middlename,
                Email = email,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            Common.Security.Security objSecurity = new(context);
            var getHandler = new CreateAdministratorHandler(logService.Object, context, cryptographyService, objSecurity, iemail.Object);
            var response = getHandler.Handle(command, new CancellationToken());

            Assert.True(response != null && response.IsCompleted && response.Result);
        }

        /// <summary>
        ///  Validate System Parameter configurations
        /// </summary> 
        [Fact]
        public void Create_Administrator_Validate_SytemParam_Test()
        {
            var lstParams = new List<SystemParam>();


            Common.Security.Security objsecurity = new(context);

            CustomException exception = Assert.Throws<CustomException>(() => objsecurity.ValidatePasswordRules(lstParams));

            Assert.Equal(GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDRULES, exception.Message);
        }


        /// <summary>
        ///  Validate System parameters configured
        /// </summary> 
        /// <param name="paramcode">System Parameters Code</param>
        /// <param name="msgError">Message error</param>
        [Theory]
        [InlineData(GlobalConstantHelpers.PASSWORDRULES, GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORD)]
        [InlineData(GlobalConstantHelpers.PASSWORDRANDOMLETTERS, GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDRANDMOLETTER)]
        [InlineData(GlobalConstantHelpers.PASSWORDMINLENGTHRULES, GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDMIN)]
        [InlineData(GlobalConstantHelpers.PASSWORDMAXLENGTHRULES, GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDMAX)]
        [InlineData("LENGTH", GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDMINMAX)]

        public void Create_Administrator_Validate_SytemParam_items_Test(string paramcode, string msgError)
        {
            List<SystemParam> lstParamsAux = SetDataMock.AddSystemParam();
            List<SystemParam> lstParams = new();

            if (paramcode == "LENGTH")
            {
                lstParams = lstParamsAux
                    .Select(s =>
                    {
                        if (s.PARAM_CODE == GlobalConstantHelpers.PASSWORDMINLENGTHRULES)
                            s.PARAM_VALUE = "1000";
                        return s;
                    }).ToList();
            }
            else
            {
                lstParams = lstParamsAux.Where(w => w.PARAM_CODE != paramcode).ToList();
            }

            Common.Security.Security objsecurity = new(context);

            CustomException exception = Assert.Throws<CustomException>(() => objsecurity.ValidatePasswordRules(lstParams));

            Assert.Equal(msgError, exception.Message);
        }

        /// <summary>
        /// This Method validate if GetPassword is correct
        /// </summary>
        [Fact]
        public void Create_Administrator_GeneratePassword()
        {
            var lstParams = context.SYSTEM_PARAM.ToList();
            Common.Security.Security objsecurity = new(context);
            var getHandler = new CreateAdministratorHandler(logService.Object, context, cryptographyService, objsecurity, iemail.Object);
            var response = getHandler.GetRandomPassword(lstParams);

            Assert.NotNull(response);
        }

        /// <summary>
        /// Validate if the method BuildRegex are correct
        /// </summary>
        [Fact]
        public void BuildRegex_Test()
        {
            Common.Security.Security objSecurity = new(context);
            List<SystemParam> lstParams = context.SYSTEM_PARAM.ToList();

            var regex = objSecurity.BuildRegex(lstParams);

            Assert.NotNull(regex);
        }

        /// <summary>
        /// Validate if the method BuildRegex are correct
        /// </summary>
        [Theory]
        [InlineData("1SG:SASASASASASASASASY")]
        [InlineData("123456")]
        public void ValidatePassword(string password)
        {
            Common.Security.Security objSecurity = new(context);
            List<SystemParam> lstParams = context.SYSTEM_PARAM.ToList();

            bool isValid = objSecurity.ValidatePassword(lstParams, password, false);

            Assert.True(!isValid);
        }
    }
}

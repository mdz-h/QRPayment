
#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-22.
// Comment: Class for unit test of ChangePassword.cs
//===============================================================================
#endregion

using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Services;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword
{
    public class ChangePasswordTest
    {
        readonly Mock<IMediator> Mediator;
        private readonly ISecurity security;
        private readonly ICryptographyService cryptographyService;
        private readonly ApplicationDbContext ApplicationDbContextFake;

        public ChangePasswordTest()
        {
            Mediator = new Mock<IMediator>();

            var fixtureSecurity = new Fixture().Customize(new AutoMoqCustomization());
            security = fixtureSecurity.Create<Common.Security.Security>();

            var fixtureCryptographyService = new Fixture().Customize(new AutoMoqCustomization());
            cryptographyService = fixtureCryptographyService.Create<CryptographyService>();

            List<Operator> OPERATOR = new List<Operator>();
            for (var count = 1; count <= 10; count++)
            {
                var operaatorId = new Guid();
                if (count == 1) operaatorId = new Guid("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c");
                if (count == 2) operaatorId = new Guid("693aaaee-256c-ed11-ade5-a04a5e6d758c");
                OPERATOR.Add(new Operator()
                {
                    OPERATOR_ID = operaatorId,
                    PERSON_ID = 1,
                    OPERATOR_STATUS_ID = 3,
                    USER_NAME = $"user_TEST {count}",
                    FL_INTRN = true,
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow,
                    MODIFIED_BY_OPERATOR_ID = null,
                    MODIFIED_DATETIME = null
                });
            }
            List<OperatorPassword> OPERATOR_PASSWORD = new List<OperatorPassword>();
            for (var count = 1; count <= 30; count++)
            {
                bool isActive = false;
                string password_test = ($"pass_{count.ToString()}");
                if (count == 1) password_test = ("c89787bb84a803a024f60df0545ae07eeccf1c11369cc4b2ec8dc3eb7b12587a"); // .s!G:3166Y
                if (count == 26) password_test = ("402132c8717707bb8c5630721e7b7361b27971bce99545c2fb3f315c073be78b"); // 1s!G:3166Y
                if (count == 30) password_test = ("ad06ce8bef5a84c88e758cceb7a69332d53c491dbcf0e74839ec37e74dab7d28"); // -s!G:3166Y
                if (count == 30) isActive = true;
                OPERATOR_PASSWORD.Add(new OperatorPassword()
                {
                    OPERATOR_PASSWORD_ID = count,
                    OPERATOR_ID = new Guid("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c"),
                    PASSWORD = password_test,
                    EXPIRATION_TIME = DateTime.UtcNow,
                    IS_ACTIVE = isActive,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow.AddDays(count),
                    MODIFIED_BY_OPERATOR_ID = null,
                    MODIFIED_DATETIME = null
                });
            }

            List<SystemParam> SYSTEM_PARAM = new List<SystemParam>();
            SYSTEM_PARAM.Add(new SystemParam() { PARAM_CODE = "PASSWORD_RULES", PARAM_VALUE = "{\"LimitExpressionOK\":5,\"Expressions\":[\"[0-9]\\u002B\",\"[A-Z]\\u002B\",\"^.{8,15}$\",\"[a-z]\\u002B\",\"[!@#$%^\\u0026*()_\\u002B=\\\\[{\\\\]};:\\u003C\\u003E|./?,-]\"]}" });
            SYSTEM_PARAM.Add(new SystemParam() { PARAM_CODE = "PASSWORD_MIN_LENGTH_RULES", PARAM_VALUE = "8" });
            SYSTEM_PARAM.Add(new SystemParam() { PARAM_CODE = "PASSWORD_MAX_LENGTH_RULES", PARAM_VALUE = "15" });
            SYSTEM_PARAM.Add(new SystemParam() { PARAM_CODE = "PASSWORD_RANDOM_LETTERS", PARAM_VALUE = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_+=\\[{\\]};:<>|./?,-" });
            SYSTEM_PARAM.Add(new SystemParam() { PARAM_CODE = "PASSWORD_EXPIRATION_TIME", PARAM_VALUE = "60" });

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            ApplicationDbContextFake = new ApplicationDbContext(options, Mediator.Object, null);
            ApplicationDbContextFake.OPERATOR.AddRange(OPERATOR);
            ApplicationDbContextFake.OPERATOR_PASSWORD.AddRange(OPERATOR_PASSWORD);
            ApplicationDbContextFake.SYSTEM_PARAM.AddRange(SYSTEM_PARAM);
            ApplicationDbContextFake.SaveChanges();
        }

        [Theory]
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "-s!G:3166Y", "1s!G:3166Y")] //True
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "123456", "1s!G:3166Y")] //False
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "", "1s!G:3166Y")] //False
        [InlineData("f1e12d08-ec6c-ed11-ade5-999999999999", "", "1s!G:3166Y")] //False        
        public void ChangePasswordAdministratorAsync_InputParams_ReturnBoolean(string UserId, string OldPassword, string NewPassword)
        {
            //Arrange            
            ChangePasswordCommand changePasswordCommand = new()
            {
                UserId = UserId,
                OldPassword = OldPassword,
                NewPassword = NewPassword,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            //Act
            var changePassword = new ChangePassword(ApplicationDbContextFake, security, cryptographyService);
            var result = changePassword.ChangePasswordAdministratorAsync(changePasswordCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            if (OldPassword == "-s!G:3166Y" && UserId == "f1e12d08-ec6c-ed11-ade5-a04a5e6d758c")
            {
                Assert.True(result.Result);                
            }
            if (OldPassword != "-s!G:3166Y" || UserId != "f1e12d08-ec6c-ed11-ade5-a04a5e6d758c")
            {
                Assert.False(result.Result);
            }

        }

        [Theory]
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "-s!G:3166Y", "1s!G:3166Y")] //True
        [InlineData("99999999-9999-9999-9999-999999999999", "-s!G:3166Y", "1s!G:3166Y")] //False        
        public void ValidateExistsAdministratorAsync_InputParams_ReturnBoolean(string UserId, string OldPassword, string NewPassword)
        {
            //Arrange            
            ChangePasswordCommand changePasswordCommand = new()
            {
                UserId = UserId,
                OldPassword = OldPassword,
                NewPassword = NewPassword,
            };

            //Act
            var changePassword = new ChangePassword(ApplicationDbContextFake, security, cryptographyService);
            var result = changePassword.ValidateExistsAdministratorAsync(changePasswordCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            if (UserId == "f1e12d08-ec6c-ed11-ade5-a04a5e6d758c")
            {
                Assert.True(result.Result);                
            }
            if (UserId != "f1e12d08-ec6c-ed11-ade5-a04a5e6d758c")
            {
                Assert.False(result.Result);                
            }

        }

        [Theory]
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "-s!G:3166Y", "1s!G:3166Y")] //True
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "123456789010", "1s!G:3166Y")] //False
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "", "1s!G:3166Y")] //False            
        public void ValidateIfPasswordIsEqualToTheCurrentPassword_InputParams_ReturnBoolean(string UserId, string OldPassword, string NewPassword)
        {
            //Arrange            
            ChangePasswordCommand changePasswordCommand = new()
            {
                UserId = UserId,
                OldPassword = OldPassword,
                NewPassword = NewPassword,
            };

            //Act
            var changePassword = new ChangePassword(ApplicationDbContextFake, security, cryptographyService);
            var result = changePassword.ValidateIfPasswordIsEqualToTheCurrentPassword(changePasswordCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            if (OldPassword == "-s!G:3166Y")
            {
                Assert.True(result.Result);
            }
            if (OldPassword != "-s!G:3166Y")
            {
                Assert.False(result.Result);
            }

        }

        [Theory]
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "-s!G:3166Y", "1s!G:3166Y")] //True and null Exception
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "#_123Test12@", "1s!G:3166Y")] //True and null Exception
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "1s!G:3166Y", "123456")] //Not null Exception Message
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "1s!G:3166Y", "abcdef")] //Not null Exception Message
        public void ValidateNewPassword_InputParams_ReturnBooleanAndCheckIfItIsNullException(string UserId, string OldPassword, string NewPassword)
        {
            //Arrange            
            ChangePasswordCommand changePasswordCommand = new()
            {
                UserId = UserId,
                OldPassword = OldPassword,
                NewPassword = NewPassword,
            };

            //Act
            var changePassword = new ChangePassword(ApplicationDbContextFake, security, cryptographyService);
            var result = changePassword.ValidateNewPassword(changePasswordCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            if (NewPassword == "1s!G:3166Y" || NewPassword == "1s!G:3166Y")
            {                
                Assert.True(result.Result);
                Assert.Null(result.Exception);
            }
            if (NewPassword == "123456" || NewPassword == "abcdef")
            {
                Assert.NotNull(result.Exception.Message);
            }

        }

        [Theory]
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "-s!G:3166Y", "1s!G:3166Y")] //False
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "-s!G:3166Y", "-s!G:3166Y")] //False
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "-s!G:3166Y", ".s!G:12sH")] //True
        [InlineData("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", "-s!G:3166Y", "%T!a:9999d")] //True  
        public void ValidateNewPasswordWithTheLastPasswords_InputParams_ReturnBoolean(string UserId, string OldPassword, string NewPassword)
        {
            //Arrange            
            ChangePasswordCommand changePasswordCommand = new()
            {
                UserId = UserId,
                OldPassword = OldPassword,
                NewPassword = NewPassword,
            };

            //Act
            var changePassword = new ChangePassword(ApplicationDbContextFake, security, cryptographyService);
            var result = changePassword.ValidateNewPasswordWithTheLastPasswords(changePasswordCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            if (NewPassword == "1s!G:3166Y" || NewPassword == "-s!G:3166Y")
            {                
                Assert.False(result.Result);
            }
            if (NewPassword == ".s!G:12sH" || NewPassword == "%T!a:9999d")
            {
                Assert.True(result.Result);                
            }

        }


    }
}

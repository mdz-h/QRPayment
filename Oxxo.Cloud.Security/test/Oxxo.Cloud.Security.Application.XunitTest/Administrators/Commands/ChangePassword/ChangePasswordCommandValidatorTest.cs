#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-22.
// Comment: Class for unit test of ChangePasswordCommandValidator.cs
//===============================================================================
#endregion
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using System.Reflection;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword
{
    public class ChangePasswordCommandValidatorTest
    {
        private Mock<IChangePassword> password;

        public ChangePasswordCommandValidatorTest()
        {
            password = new Mock<IChangePassword>();
        }

        [Fact]
        public void ChangePasswordCommandValidator_ValidateConstructor()
        {
            this.password = new Mock<IChangePassword>();
            //Act  
            ChangePasswordCommandValidator changePasswordCommandValidator = new ChangePasswordCommandValidator(password.Object);
            //Assert
            Assert.NotNull(changePasswordCommandValidator);
        }


        [Theory]
        [InlineData("3D30156B-2446-4455-EBD7-08DAE3C87C15")]
        [InlineData("")]
        [InlineData(null)]
        public void ValidateTypeUserId_InputUserId_ReturnBool(string UserId)
        {
            // Arrange
            ChangePasswordCommand changePasswordCommand = new ChangePasswordCommand()
            {
                UserId = UserId,
            };

            Type type = typeof(ChangePasswordCommandValidator);
            var changePasswordCommandValidator = Activator.CreateInstance(type, password.Object);

            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "ValidateTypeUserId" && x.IsPrivate).First();

            //Act
            var validateIfExistsDevices = method.Invoke(changePasswordCommandValidator, new object[] { changePasswordCommand });

            //Assert
            if (changePasswordCommand.UserId == "3D30156B-2446-4455-EBD7-08DAE3C87C15")
            {
                Assert.Equal(true, validateIfExistsDevices);
            }
            if (changePasswordCommand.UserId == "" || changePasswordCommand.UserId is null)
            {
                Assert.Equal(false, validateIfExistsDevices);
            }
        }

        [Theory]
        [InlineData("3D30156B-2446-4455-EBD7-08DAE3C87C15", "-s!G:3166Y", "1s!G:3166Y")]
        [InlineData("", "-s!G:3166Y", "1s!G:3166Y")]
        [InlineData(null, null, null)]
        public void ValidatesStructureForChangePasswordOfAdministrators_inputOaramsReqeust(string UserId, string OldPassword, string NewPassword)
        {
            // Arrange
            ChangePasswordCommand changePasswordCommand = new ChangePasswordCommand()
            {
                UserId = UserId,
                OldPassword = OldPassword,
                NewPassword = NewPassword,
            };

            Type type = typeof(ChangePasswordCommandValidator);
            var changePasswordCommandValidator = Activator.CreateInstance(type, password.Object);

            //Act
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "ValidateIfExistsAdministratorAsync" && x.IsPrivate).First();
            var ValidateIfExistsAdministratorAsync = method.Invoke(changePasswordCommandValidator, new object[] { changePasswordCommand, new CancellationToken() });

            method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "ValidateIfPasswordIsEqualToTheCurrentPassword" && x.IsPrivate).First();
            var ValidateIfPasswordIsEqualToTheCurrentPassword = method.Invoke(changePasswordCommandValidator, new object[] { changePasswordCommand, new CancellationToken() });

            method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "ValidateNewPassword" && x.IsPrivate).First();
            var ValidateNewPassword = method.Invoke(changePasswordCommandValidator, new object[] { changePasswordCommand, new CancellationToken() });

            method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "ValidateNewPasswordWithTheLastPasswords" && x.IsPrivate).First();
            var ValidateNewPasswordWithTheLastPasswords = method.Invoke(changePasswordCommandValidator, new object[] { changePasswordCommand, new CancellationToken() });

            //Assert
            Assert.NotNull(ValidateIfExistsAdministratorAsync);
            Assert.NotNull(ValidateIfPasswordIsEqualToTheCurrentPassword);
            Assert.NotNull(ValidateNewPassword);
            Assert.NotNull(ValidateNewPasswordWithTheLastPasswords);

        }

    }
}

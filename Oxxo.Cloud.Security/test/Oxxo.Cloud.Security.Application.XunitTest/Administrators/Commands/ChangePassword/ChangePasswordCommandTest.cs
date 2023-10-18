#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-22.
// Comment: Class for unit test of ChangePasswordCommand.cs
//===============================================================================
#endregion
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword
{
    public class ChangePasswordCommandTest
    {
        private readonly Mock<ILogService> logService;
        private readonly Mock<IChangePassword> iChangePassword;

        public ChangePasswordCommandTest()
        {
            this.logService = new Mock<ILogService>();
            this.iChangePassword = new Mock<IChangePassword>();
        }

        [Theory]
        [InlineData("3D30156B-2446-4455-EBD7-08DAE3C87C15", "-s!G:3166Y", "1s!G:3166Y")]
        [InlineData("", "-s!G:3166Y", "1s!G:3166Y")]
        [InlineData("3D30156B-2446-4455-EBD7-08DAE3C87C15", "", "1s!G:3166Y")]
        [InlineData("3D30156B-2446-4455-EBD7-08DAE3C87C15", "-s!G:3166Y", "")]
        [InlineData("", "", "")]
        [InlineData(null, null, null)]
        public void ChangePasswordCommand_InputRequest_ReturnNotNullIsCompletedAndReturnFalse(string UserId, string OldPassword, string NewPassword)
        {

            //Arrange            
            ChangePasswordCommand changePasswordCommand = new ChangePasswordCommand()
            {
                UserId = UserId,
                OldPassword = OldPassword,
                NewPassword = NewPassword,
            };

            //Act
            var changePasswordHandler = new ChangePasswordHandler(logService.Object, iChangePassword.Object);
            var response = changePasswordHandler.Handle(changePasswordCommand, new CancellationToken());
            
            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsCompleted);
            Assert.False(response.Result);

        }
    }
}

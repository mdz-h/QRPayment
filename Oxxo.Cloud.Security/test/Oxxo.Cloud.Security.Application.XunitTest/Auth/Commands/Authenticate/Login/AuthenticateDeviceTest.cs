using Moq;
using Oxxo.Cloud.Security.Application.Auth.Commands.Login;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate.Login
{
    public class AuthenticateDeviceTest
    {
        private readonly Mock<ITokenGenerator> tokenGenerator;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IAuthenticateQuery> authenticateQuery;
        private readonly Mock<ILogService> logService;

        public AuthenticateDeviceTest()
        {
            tokenGenerator = new Mock<ITokenGenerator>();
            context = new Mock<IApplicationDbContext>();
            authenticateQuery = new Mock<IAuthenticateQuery>();
            logService = new Mock<ILogService>();
        }

        [Fact]
        public void AuthenticateDevice_ReturnNotNull()
        {
            /// Act
            var authenticate = new Mock<AuthenticateDevice>(context.Object, tokenGenerator.Object, authenticateQuery.Object);

            //Assert
            Assert.NotNull(authenticate);
        }

        [Theory]
        [InlineData("device", "3c2b9756c80cc4f96bff5c1ec0210dbaa62dbcd4e68bd1704ebd01ca2583a6b0")]
        public void AuthenticateDevice_InputDataDevice_ReturnExecption(string typeAuth, string deviceKey)
        {
            DeviceDto authDto = new DeviceDto()
            {
                TypeAuth = typeAuth,
                Id = deviceKey
            };

            /// Act
            var authenticateDeviceMock = new AuthenticateDevice(context.Object, tokenGenerator.Object, authenticateQuery.Object, logService.Object, authDto);

            //Arrange
            var authenticate = authenticateDeviceMock.Auth(new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.NotNull(authenticate.Exception);
            Assert.NotNull(authenticate.Exception?.InnerException);
        }
    }
}

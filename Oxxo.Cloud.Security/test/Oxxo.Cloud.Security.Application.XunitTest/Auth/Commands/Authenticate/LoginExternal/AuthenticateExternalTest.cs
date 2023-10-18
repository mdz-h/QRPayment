using Moq;
using Oxxo.Cloud.Security.Application.Auth.Commands.Login;
using Oxxo.Cloud.Security.Application.Auth.Commands.LoginExternal;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate.LoginExternal
{
    public class AuthenticateExternalTest
    {
        private readonly Mock<ITokenGenerator> tokenGenerator;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<ILogService> logService;

        public AuthenticateExternalTest()
        {
            tokenGenerator = new Mock<ITokenGenerator>();
            context = new Mock<IApplicationDbContext>();
            logService = new Mock<ILogService>();
        }

        [Fact]
        public void AuthenticateExternal_ReturnNotNull()
        {
            /// Act
            var authenticate = new Mock<AuthenticateDevice>(context.Object, tokenGenerator.Object);

            //Assert
            Assert.NotNull(authenticate);
        }

        [Theory]
        [InlineData("external", "12345")]
        public void AuthenticateExternal_InputDataDevice_ReturnExecption(string typeAuth, string apiKey)
        {
            ExternalDto authDto = new ExternalDto()
            {
                TypeAuth = typeAuth,
                Id = apiKey
            };

            /// Act
            var authenticateExternal = new AuthenticateExternal(context.Object, tokenGenerator.Object, logService.Object, authDto);

            //Arrange
            var authenticate = authenticateExternal.Auth(new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.NotNull(authenticate.Exception);
            Assert.NotNull(authenticate.Exception?.InnerException);
        }
    }
}

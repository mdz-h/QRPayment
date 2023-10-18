using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Refresh
{
    public class RefreshTokenCommandTest
    {
        private readonly Mock<IApplicationDbContext> context;

        private readonly Mock<ITokenGenerator> tokenGenerator;

        private readonly Mock<IAuthenticateQuery> authenticateQuery;
        private readonly Mock<ILogService> logService;

        public RefreshTokenCommandTest()
        {
            context = new Mock<IApplicationDbContext>();
            tokenGenerator = new Mock<ITokenGenerator>();
            logService = new Mock<ILogService>();
            authenticateQuery = new Mock<IAuthenticateQuery>();
        }

        [Fact]
        public void RefreshToken_ReturnNotNull()
        {
            //Arrange
            var authenticate = new TokenCommandHandler(context.Object, tokenGenerator.Object, authenticateQuery.Object, logService.Object);
            //Assert
            Assert.NotNull(authenticate);
        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1PTiIsImp0aSI6IjUwUktTIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjI0YzE5NWZkNjIwOWJkZWI4Y2ZiOWQ2M2FkOWZlNGFjOTNjY2Y2M2Y0ZjZiMmViZDljZWMwYjY0NGQ0NTQyYWIiLCJOdW1iZXJEZXZpY2UiOiIxIiwiZXhwIjoxNjY3ODIyMTY5LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MiIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzcyIn0.Zvm6O6x0GDl7eHI_q0pjiyiw6Dcjn-J8Rneg3GB2sFc", "15GwPNrONQW4T4x6kUnVCymOWbl/JjAvcF1C6+XIIfxrb5Y84tgo1WNuyMU1YUoECMikc4jkGWh0na/A2nTWSw==")]
        public void RefreshToken_InputDataAuthenticate_ReturnExecption(string token, string tokenRefresh)
        {
            /// Act
            var authenticate = new TokenCommandHandler(context.Object, tokenGenerator.Object, authenticateQuery.Object, logService.Object);
            RefreshTokenCommand command = new() { BearerToken = token, RefreshToken = tokenRefresh };

            //Arrange           
            var response = authenticate.Handle(command, new CancellationToken());

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Exception);
            Assert.NotNull(response.Exception?.InnerException);
        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1PTiIsImp0aSI6IjUwUktTIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjI0YzE5NWZkNjIwOWJkZWI4Y2ZiOWQ2M2FkOWZlNGFjOTNjY2Y2M2Y0ZjZiMmViZDljZWMwYjY0NGQ0NTQyYWIiLCJOdW1iZXJEZXZpY2UiOiIxIiwiZXhwIjoxNjY3ODIyMTY5LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MiIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzcyIn0.Zvm6O6x0GDl7eHI_q0pjiyiw6Dcjn-J8Rneg3GB2sFc", "15GwPNrONQW4T4x6kUnVCymOWbl/JjAvcF1C6+XIIfxrb5Y84tgo1WNuyMU1YUoECMikc4jkGWh0na/A2nTWSw==")]
        public async Task RefreshToken_InputDataAuthenticateExternal_ReturnIsValid(string token, string tokenRefresh)
        {
            //Arrange
            RefresTokenCommandValidator commandValidator = new RefresTokenCommandValidator(authenticateQuery.Object);
            var cmd = new RefreshTokenCommand() { BearerToken = token, RefreshToken = tokenRefresh };
            //Act
            var commandResponse = await commandValidator.ValidateAsync(cmd);
            //Act
            Assert.False(commandResponse.IsValid);
        }
    }
}

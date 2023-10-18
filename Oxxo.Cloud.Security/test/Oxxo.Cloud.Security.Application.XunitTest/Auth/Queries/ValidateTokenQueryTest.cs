using Moq;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Auth.Queries
{
    public class ValidateTokenQueryTests
    {
        private readonly Mock<IAuthenticateQuery> authenticateQuery;
        private readonly Mock<ILogService> logService;
        private readonly Mock<ITokenGenerator> tokenGenerator;
        public ValidateTokenQueryTests()
        {
            authenticateQuery = new Mock<IAuthenticateQuery>();
            logService = new Mock<ILogService>();
            tokenGenerator = new Mock<ITokenGenerator>();
        }

        [Fact]
        public void ValidateTokenQueryHandler_InputTokenNull_ReturnsCustomException()
        {
            /// Arrange
            ValidateTokenQueryHandler validationTokenQueryHandler = new ValidateTokenQueryHandler(authenticateQuery.Object, logService.Object, tokenGenerator.Object);
            ValidateTokenQuery? validateTokenQuery = null;
            /// Act          
            var response = () => validationTokenQueryHandler.Handle(validateTokenQuery, new CancellationToken());

            /// Assert
            Assert.ThrowsAsync<CustomException>(response);
        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1PTiIsImp0aSI6IjUwUktTIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjI0YzE5NWZkNjIwOWJkZWI4Y2ZiOWQ2M2FkOWZlNGFjOTNjY2Y2M2Y0ZjZiMmViZDljZWMwYjY0NGQ0NTQyYWIiLCJOdW1iZXJEZXZpY2UiOiIxIiwiZXhwIjoxNjY3ODIyMTY5LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MiIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzcyIn0.Zvm6O6x0GDl7eHI_q0pjiyiw6Dcjn-J8Rneg3GB2sFc")]
        public async Task ValidateTokenQueryHandlerd_InputToken_ReturnIsValid(string token)
        {
            //Arrange
            ValidateTokenQueryValidator validation = new ValidateTokenQueryValidator();
            var cmd = new ValidateTokenQuery() { BearerToken = token };
            //Act
            var commandResponse = await validation.ValidateAsync(cmd);
            //Act
            Assert.False(commandResponse.IsValid);
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Oxxo.Cloud.Security.Application.Auth.Queries;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;
using Oxxo.Cloud.Security.Infrastructure.Services;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate
{
    public class AuthenticateCommandTest
    {
        private readonly Mock<ICryptographyService> cryptographyService;
        private readonly Mock<ILogService> logService;
        private readonly AuthenticateQuery authenticateQuery;
        private readonly TokenGenerator tokenGenerator;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> Mediator;
        private readonly string key = "ll@v3DeSegur1dad231";
        private readonly string issuer = "https://localhost:44372";
        private readonly string audience = "https://localhost:44372";
        private readonly Mock<ISecurity> security;
        public AuthenticateCommandTest()
        {                 
            cryptographyService = new Mock<ICryptographyService>();
            logService = new Mock<ILogService>();
            tokenGenerator = new TokenGenerator(key, issuer, audience);
            Mediator = new Mock<IMediator>();
            security = new Mock<ISecurity>();
            List<SystemParam> lstSystemParam = SetDataMock.AddSystemParam();
            List<Operator> lstOperator = SetDataMock.AddOperator();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            context = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.SYSTEM_PARAM.AddRange(lstSystemParam);
            context.OPERATOR.AddRange(lstOperator);
            context.OPERATOR_PASSWORD.AddRange(AddOperatorPassword());
            context.STORE_PLACE.AddRange(SetDataMock.AddStorePlaces());
            context.SaveChanges();
            authenticateQuery = new AuthenticateQuery(context, cryptographyService.Object);
        }
       

        [Fact]
        public void AuthenticateCommand_ReturnNotNull()
        {
            //Arrange
            var authenticate = new AuthenticateHandler(logService.Object, context, tokenGenerator, authenticateQuery, cryptographyService.Object, security.Object);
            //Assert
            Assert.NotNull(authenticate);
        }

        [Theory]
        [InlineData("GALIELOHERNANDE", "", "24c195fd6209bdeb8cfb9d63ad9fe4ac93ccf63f4f6b2ebd9cec0b644d4542ab")]
        public void AuthenticateCommand_InputDataAuthenticate_ReturnExecption(string id, string typeAuth, string deviceKey)
        {
            /// Act
            AuthenticateCommand command = new() { Id = id, TypeAuth = typeAuth, DeviceKey = deviceKey };

            //Arrange
            var authenticateHandler = new AuthenticateHandler(logService.Object, context, tokenGenerator, authenticateQuery, cryptographyService.Object, security.Object);
            var response = authenticateHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.NotNull(authenticateHandler);
            Assert.NotNull(response.Exception);
            Assert.NotNull(response.Exception?.InnerException);
        }

        [Theory]
        [InlineData("GALIELOHERNANDE", "device", "24c195fd6209bdeb8cfb9d63ad9fe4ac93ccf63f4f6b2ebd9cec0b644d4542ab")]
        public async Task AuthenticateCommand_InputDataAuthenticateDevice_ReturnIsValid(string id, string typeAuth, string deviceKey)
        {
            //Arrange
            AuthenticateCommandValidator commandValidator = new AuthenticateCommandValidator(authenticateQuery, cryptographyService.Object);
            var cmd = new AuthenticateCommand() { Id = id, TypeAuth = typeAuth, DeviceKey = deviceKey };
            //Act
            var commandResponse = await commandValidator.ValidateAsync(cmd);
            //Act
            Assert.False(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("DEMO", "external", "12345")]
        public async Task AuthenticateCommand_InputDataAuthenticateExternal_ReturnIsValid(string id, string typeAuth, string apiKey)
        {
            //Arrange
            AuthenticateCommandValidator commandValidator = new AuthenticateCommandValidator(authenticateQuery, cryptographyService.Object);
            var cmd = new AuthenticateCommand() { Id = id, TypeAuth = typeAuth, ApiKey = apiKey };
            //Act
            var commandResponse = await commandValidator.ValidateAsync(cmd);
            //Act
            Assert.False(commandResponse.IsValid);
        }


        [Theory]
        [InlineData("user", "morales123", "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e2", "CrPlace1", "CrStore1")]
        [InlineData("user", "zavala8472", "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e1", "CrPlace1", "CrStore1")]
        [InlineData("user", "abrego52342", "785f17fd1f7b70c9929f4c709274eec0fef6afe676b4c27dff065b8a221692c2", "CrPlace1", "CrStore1")]
        public void AuthenticateUserCreateNewToken_InputDataUserPassword_ReturnAuthResponse(string typeAuth, string user, string password, string crPlace, string crStore)
        {
            /// Act
            AuthenticateCommand command = new() { TypeAuth = typeAuth, User = user, Password = password, Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398", CrPlace = crPlace, CrStore = crStore };

            //Arrange
            var authenticateHandler = new AuthenticateHandler(logService.Object, context, tokenGenerator, authenticateQuery, cryptographyService.Object, security.Object);
            var response = authenticateHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result.Token);
            Assert.IsType<AuthResponse>(response.Result);
            Assert.NotNull(response.Result.Token);
            Assert.NotEmpty(response.Result.Token);
            Assert.True(response.Result.Auth);
        }

        private static List<OperatorPassword> AddOperatorPassword()
        {
            return new List<OperatorPassword>()
            {
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("63df3ad9-0a5f-490b-bd4f-424b3bbd5c49"),
                    PASSWORD = "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e2",
                    EXPIRATION_TIME = DateTime.UtcNow.AddDays(10),
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                },
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("33df3ad9-0a5f-490b-bd4f-424b3bbd5c11"),
                    PASSWORD = "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e1",
                    EXPIRATION_TIME = DateTime.UtcNow.AddDays(10),
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                },
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("380CC0CB-EFD2-4FFB-7E57-08DAFFB6CD6C"),
                    PASSWORD = "785f17fd1f7b70c9929f4c709274eec0fef6afe676b4c27dff065b8a221692c2",
                    EXPIRATION_TIME = DateTime.UtcNow.AddDays(10),
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                }
            };
        }
    }
}

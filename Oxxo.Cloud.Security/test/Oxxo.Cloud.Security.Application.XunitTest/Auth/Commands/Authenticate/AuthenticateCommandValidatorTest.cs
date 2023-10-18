using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Oxxo.Cloud.Security.Application.Auth.Queries;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;
using Oxxo.Cloud.Security.Infrastructure.Services;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate
{
    public class AuthenticateCommandValidatorTest
    {
        private readonly CryptographyService cryptographyService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> Mediator;
        private readonly AuthenticateQuery authenticateQuery;
        private readonly string publicKey = "<RSAKeyValue><ModulusfZeLYGkkCYvDLqtLnqjxAE4i90uG/+YUaRvrdOWdQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private readonly string privateKey = "<RSAKeyValue><Modulus>rAE4i90uG/+YUaRvrdOWdQ==</Modulus><Exponent>AQAB</Exponent><P>xd5dr9/ZDt</D></RSAKeyValue>";

        public AuthenticateCommandValidatorTest()
        {
            Mediator = new Mock<IMediator>();
            cryptographyService = new CryptographyService(publicKey, privateKey);           
            List<Operator> lstOperator = SetDataMock.AddOperator();            

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            context = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor());            
            context.OPERATOR.AddRange(lstOperator);
            context.STORE_PLACE.AddRange(AddStorePlaceMockData());
            context.SaveChanges();
            authenticateQuery = new AuthenticateQuery(context, cryptographyService);
        }

        [Theory]
        [InlineData("user", "morales123", "5?*sihK[Qx")]
        [InlineData("user", "zavala8472", "4*?s1rj]Vx")]
        [InlineData("user", "abrego52342", "3&?g8ar]sd")]
        public async Task UserLoginValidation_InputDataUserPassword_ReturnTrue(string typeAuth, string user, string password)
        {
            /// Arrange
            AuthenticateCommandValidator commandValidator = new(authenticateQuery,cryptographyService);
            List<OperatorPassword> lstOperatorPassword = AddOperatorPassword();
            context.OPERATOR_PASSWORD.AddRange(lstOperatorPassword);
            context.SaveChanges();
            AuthenticateCommand authenticate = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
            };

            /// Act
            var commandResponse = await commandValidator.ValidateAsync(authenticate);

            //Assert            
            Assert.True(commandResponse.IsValid);
            Assert.Empty(commandResponse.Errors);
        }

        [Theory]
        [InlineData("user", "morales123", "7362846482")]
        [InlineData("user", "zavala8472", "hdjJFH3YSF")]
        [InlineData("user", "abrego52342", "7HDJ3047N3")]
        public async Task UserLoginValidation_InputDataUserPassword_ReturnFalse(string typeAuth, string user, string password)
        {
            /// Arrange
            AuthenticateCommandValidator commandValidator = new(authenticateQuery, cryptographyService);
            List<OperatorPassword> lstOperatorPassword = AddOperatorPassword();
            context.OPERATOR_PASSWORD.AddRange(lstOperatorPassword);
            context.SaveChanges();
            AuthenticateCommand authenticate = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
            };

            /// Act
            var commandResponse = await commandValidator.ValidateAsync(authenticate);

            //Assert            
            Assert.False(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("user", "morales123", "5?*sihK[Qx")]
        [InlineData("user", "zavala8472", "4*?s1rj]Vx")]
        [InlineData("user", "abrego52342", "3&?g8ar]sd")]
        public async Task UserLoginValidation_InputDataUserPasswordExpirated_ReturnFalse(string typeAuth, string user, string password)
        {
            /// Arrange
            AuthenticateCommandValidator commandValidator = new(authenticateQuery, cryptographyService);
            List<OperatorPassword> lstOperatorPassword = AddOperatorPasswordExpireted();
            context.OPERATOR_PASSWORD.AddRange(lstOperatorPassword);
            context.SaveChanges();
            AuthenticateCommand authenticate = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
            };

            /// Act
            var commandResponse = await commandValidator.ValidateAsync(authenticate);

            //Assert            
            Assert.False(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("user_xpos", "morales123", "sPrhuoUr6yg2C1yh4Q9ChN+uEGVXWGjwSUnkTGQ3F8eMQ73+2uIyNg4WeYEtPpGB68NnhuoxL4Q2JzVRp4+GYjxaxLnymGcgvksViclc6M7dbHnydpU9Zyoslhi44suOejyZTucp0r3quYf+4vftdx47QsslFbQH7KYbDW4wtxcBX+RGlswrokb8kLg1yHKH0RtDxFOUAOhQYdEjkY8m8Lr05NhkX7e/tl+FNj9C2wMCfZT0RFyMMItFFA9P7/BEOQPW8dJoaMlr1CVi++wsRlGSFC/TeC0J6yYrHAno3f7NJCHVzuOYHXBzq+Uj1Q0ORAINAzSO02zwYQ76tDfWDA==",
       "509YK", "10MAN", "2", "20230525", "20230525", true)]
        public async Task UserXposLoginValidation_InputDataUserPassword_ReturnTrue(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        {
            /// Arrange
            AuthenticateCommandValidator commandValidator = new(authenticateQuery, cryptographyService);
            List<OperatorPassword> lstOperatorPassword = AddOperatorPassword();
            context.OPERATOR_PASSWORD.AddRange(lstOperatorPassword);
            context.SaveChanges();
            AuthenticateCommand authenticate = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
                CrStore = crStore,
                CrPlace = crPlace,
                Till = till,
                AdministrativeDate = administrativeDate,
                ProcessDate = processDate,
                IsInternal = isInternal
            };

            /// Act
            var commandResponse = await commandValidator.ValidateAsync(authenticate);

            //Assert            
            Assert.True(commandResponse.IsValid);
            Assert.Empty(commandResponse.Errors);
        }

        private static List<OperatorPassword> AddOperatorPassword()
        {
            return new List<OperatorPassword>()
            {
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("63df3ad9-0a5f-490b-bd4f-424b3bbd5c49"),
                    PASSWORD = "fec49ccaf27549f68ed81837eb3933d2dea17b70e763e98b48e5d4b1f14f5e43",
                    EXPIRATION_TIME = DateTime.UtcNow.AddDays(10),
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                },
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("33df3ad9-0a5f-490b-bd4f-424b3bbd5c11"),
                    PASSWORD = "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e2",
                    EXPIRATION_TIME = DateTime.UtcNow.AddDays(10),
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                },
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("380CC0CB-EFD2-4FFB-7E57-08DAFFB6CD6C"),
                    PASSWORD = "95b35f466d35062d9f87375457a5ba87485194d7258247352c9ed5cc31ef5c33",
                    EXPIRATION_TIME = DateTime.UtcNow.AddDays(10),
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                }
            };
        }

        private static List<OperatorPassword> AddOperatorPasswordExpireted()
        {
            return new List<OperatorPassword>()
            {
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("63df3ad9-0a5f-490b-bd4f-424b3bbd5c49"),
                    PASSWORD = "fec49ccaf27549f68ed81837eb3933d2dea17b70e763e98b48e5d4b1f14f5e43",
                    EXPIRATION_TIME = DateTime.UtcNow,
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                },
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("33df3ad9-0a5f-490b-bd4f-424b3bbd5c11"),
                    PASSWORD = "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e2",
                    EXPIRATION_TIME = DateTime.UtcNow,
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                },
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("380CC0CB-EFD2-4FFB-7E57-08DAFFB6CD6C"),
                    PASSWORD = "95b35f466d35062d9f87375457a5ba87485194d7258247352c9ed5cc31ef5c33",
                    EXPIRATION_TIME = DateTime.UtcNow,
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                }
            };
        }

        private static List<StorePlace> AddStorePlaceMockData()
        {
            List<StorePlace> PLACES = new()
            {
                new StorePlace()
                {
                    STORE_PLACE_ID = 1,
                    CR_STORE = "509YK",
                    CR_PLACE = "10MAN",
                    IS_ACTIVE = true
                },
                new StorePlace()
                {
                    STORE_PLACE_ID = 2,
                    CR_STORE = "50D49",
                    CR_PLACE = "10MAN",
                    IS_ACTIVE = true
                },
                new StorePlace()
                {
                    STORE_PLACE_ID = 3,
                    CR_STORE = "507VH",
                    CR_PLACE = "10MAN",
                    IS_ACTIVE = true
                }
            };
            return PLACES;
        }
    }
}

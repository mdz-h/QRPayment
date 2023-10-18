using Moq;
using Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate.LoginUser;
using Oxxo.Cloud.Security.Application.Auth.Commands.Login;
using Oxxo.Cloud.Security.Application.Auth.Commands.LoginExternal;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate
{
    public class AuthenticateFactoryTest
    {
        private readonly Mock<ITokenGenerator> tokenGenerator;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IAuthenticateQuery> authenticateQuery;
        private readonly Mock<ILogService> logService;
        private readonly Mock<ICryptographyService> cryptographyService;
        private readonly Mock<ISecurity> security;
        public AuthenticateFactoryTest()
        {
            tokenGenerator = new Mock<ITokenGenerator>();
            context = new Mock<IApplicationDbContext>();
            authenticateQuery = new Mock<IAuthenticateQuery>();
            logService = new Mock<ILogService>();
            cryptographyService = new Mock<ICryptographyService>();
            security = new Mock<ISecurity>();
        }

        [Theory]
        [InlineData("device")]
        public void AuthenticateFactory_ReturnTypeAuthenticateDevice(string typeAuth)
        { 
            var typeExpected = typeof(AuthenticateDevice);

            AuthenticateCommand command = new()
            {
                TypeAuth = typeAuth
            };

            //Arrange
            var authenticate = command.GetAuthenticateType(context.Object, tokenGenerator.Object, authenticateQuery.Object, logService.Object, cryptographyService.Object, security.Object);

            //Assert
            Assert.IsType(typeExpected.GetType(), authenticate.GetType());
        }

        [Theory]
        [InlineData("external")]
        public void AuthenticateFactory_ReturnTypeAuthenticateExternal(string typeAuth)
        {
            /// Act
            var typeExpected = typeof(AuthenticateExternal);

            AuthenticateCommand command = new()
            {
                TypeAuth = typeAuth
            };

            //Arrange
            var authenticate = command.GetAuthenticateType(context.Object, tokenGenerator.Object, authenticateQuery.Object, logService.Object, cryptographyService.Object, security.Object);

            //Assert
            Assert.IsType(typeExpected.GetType(), authenticate.GetType());
        }

        [Theory]
        [InlineData("user_xpos", "morales123", "uqE4zw0lKzmHFui98AI2ypCasOa25fBo+lh0SSvVtDudJoF5wG2RI8y0PrU+C11Bvt59CS3WU7bLyLSUfv6M/j9csTv8OmqtZwclTjwUhLtqk4759joTOmWAKDkgDkfLg3tcI8+WfXEgpx2sLlofsaQ2J2Cy6zRdc+IZ5uIJovX8rTqkUP2gUgyzjYJ8rKxq5wbTygFALeoLK7o2bp0znEcLE/o98AJvXrgZruFg3A6uJRsZJtAmzAr2wIe1hjTpsHYmPMiJrmSCQUBt2gT8O7lryW8Ve7yGitCwvwux+gEdyn2zmy3/UJiH3Mc4OZUPvCZYioq3dTCZkFmoo0qZfg==",
          "509YK", "10MAN", "2", "20230525", "20230525", false)]
        public void AuthenticateFactory_ReturnTypeAuthenticateUserXpos(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        {
            /// Act
            var typeExpected = typeof(AuthenticateUser);

            AuthenticateCommand command = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                CrStore = crStore,
                CrPlace = crPlace,
                Till = till,
                AdministrativeDate = administrativeDate,
                ProcessDate = processDate,
                IsInternal = isInternal
            };

            //Arrange
            var authenticate = command.GetAuthenticateType(context.Object, tokenGenerator.Object, authenticateQuery.Object, logService.Object, cryptographyService.Object, security.Object);

            //Assert
            Assert.IsType(typeExpected.GetType(), authenticate.GetType());
        }
    }
}

#region File Information
//===============================================================================
// Author:  OXXO MID MÉXICO CAAMAL (NEORIS).
// Date:    13/12/2022.
// Comment: Administrators.
//===============================================================================
#endregion 


using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Auth.Queries;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.CreateAdministrators
{
    public class CreateAdministratorValidatorTest
    {
        private readonly Mock<ILogService> logService;
        private readonly Mock<ITokenGenerator> tokenGenerator;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<ICryptographyService> cryptoGraph;

        /// <summary>
        /// Constructor Class
        /// </summary>
        public CreateAdministratorValidatorTest()
        {
            logService = new Mock<ILogService>();
            tokenGenerator = new Mock<ITokenGenerator>();
            mediator = new Mock<IMediator>();
            cryptoGraph = new Mock<ICryptographyService>();

            List<SystemParam> lstSystemParam = SetDataMock.AddSystemParam();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
              .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());

            context.SYSTEM_PARAM.AddRange(lstSystemParam);
            context.SaveChanges();
        }

        /// <summary>
        /// Validate if de email is correct
        /// </summary>
        /// <param name="nickname">this param is fake and your use is to get a guid</param>
        /// <param name="nickname">Nick Name</param>
        /// <param name="username">User Name</param>
        /// <param name="middlename">Middle Name</param>
        /// <param name="lastnamepat">Last name paternal</param>
        /// <param name="lastnamemat">last name maternal</param>
        /// <param name="email">email address</param>
        /// <param name="msgError">message error</param>
        [Theory]
        [InlineData("OXXO", "OXXO", "MID", "MÉXICO", "Caamal", "OXXO.MÉXICOneoris.com", GlobalConstantMessages.EMAILINVALID)]
        [InlineData("OXXO", "OXXO", "MID", "MÉXICO", "Caamal", "OXXO.MÉXICO@neoris..co", GlobalConstantMessages.EMAILINVALID)]
        [InlineData("OXXO", "OXXO", "MID", "MÉXICO", "Caamal", "@OXXO.MÉXICOneoris.com", GlobalConstantMessages.EMAILINVALID)]
        [InlineData("OXXO", "OXXO", "MID", "MÉXICO", "Caamal", "OXXO.MÉXICOneoris.com@", GlobalConstantMessages.EMAILINVALID)]
        public async Task CreateAdministratorsValidatorEmail(string nickname, string username, string middlename, string lastnamepat, string lastnamemat, string email, string msgError)
        {

            AuthenticateQuery auth = new(context, cryptoGraph.Object);
            CreateAdministratorCommandValidator commandValidator = new(context, auth, logService.Object, tokenGenerator.Object);
            CreateAdministratorCommand command = new()
            {
                Name = username,
                UserName = nickname,
                LastNamePat = lastnamepat,
                LastNameMat = lastnamemat,
                MiddleName = middlename,
                Email = email,
                BearerToken = SetDataMock.Token
            };
            var commandResponse = await commandValidator.ValidateAsync(command);

            msgError = string.Format(msgError, email);

            bool isValid = commandResponse.Errors.Any(a => a.ToString() == msgError);

            Assert.True(isValid);
        }

        /// <summary>
        /// Validate if de input id is correct
        /// </summary>
        /// <param name="nickname">Nick Name</param>
        /// <param name="username">User Name</param>
        /// <param name="middlename">Middle Name</param>
        /// <param name="lastnamepat">Last name paternal</param>
        /// <param name="lastnamemat">last name maternal</param>
        /// <param name="email">email address</param>
        /// <param name="msgError">message error</param>
        [Theory]
        [InlineData("", "OXXO", "MID", "MÉXICO", "Caamal", "OXXO.MÉXICO@neoris.com", GlobalConstantMessages.USERNAMEMENOTEMPTY)]
        [InlineData("OXXO", "", "MID", "MÉXICO", "Caamal", "OXXO.MÉXICO@neoris.com", GlobalConstantMessages.NAMENOTEMPTY)]
        [InlineData("OXXO", "OXXO", "MIDnel", "", "Caamal", "OXXO.MÉXICO@neoris.com", GlobalConstantMessages.LASTNAMEPATNOTEMPTY)]
        [InlineData("OXXO", "OXXO", "MIDnel", "MÉXICO", "Caamal", "", GlobalConstantMessages.EMAILNOTEMPTY)]

        public async Task CreateAdministratorsValidatorInput(string nickname, string username, string middlename, string lastnamepat, string lastnamemat, string email, string msgError)
        {

            AuthenticateQuery auth = new(context, cryptoGraph.Object);
            CreateAdministratorCommandValidator commandValidator = new(context, auth, logService.Object, tokenGenerator.Object);
            CreateAdministratorCommand command = new()
            {
                UserName = nickname,
                Name = username,
                LastNamePat = lastnamepat,
                LastNameMat = lastnamemat,
                MiddleName = middlename,
                Email = email,
                BearerToken = SetDataMock.Token
            };
            var commandResponse = await commandValidator.ValidateAsync(command);


            bool isValid = commandResponse.Errors.Any(a => a.ToString() == msgError);

            Assert.True(isValid);
        }
    }
}

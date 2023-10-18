#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    08/12/2022.
// Comment: Administrators.
//===============================================================================
#endregion 

using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.UpdateAdministrators
{
    /// <summary>
    /// Test class to validate administrators
    /// </summary>
    public class UpdateAdministratorsTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;

        /// <summary>
        /// Constructor Class
        /// </summary>
        public UpdateAdministratorsTest()
        {
            logService = new Mock<ILogService>();
            mediator = new Mock<IMediator>();

            List<Person> lstPerson = SetDataMock.AddPerson();
            List<OperatorStatus> lstOperatorStatus = SetDataMock.AddStatusData();
            List<Operator> lstOperator = SetDataMock.AddOperator();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.PERSON.AddRange(lstPerson);
            context.OPERATOR_STATUS.AddRange(lstOperatorStatus);
            context.OPERATOR.AddRange(lstOperator);
            context.SaveChanges();
        }

        /// <summary>
        ///  Validate if the handler save changes
        /// </summary>
        /// <param name="nickname">Nick Name</param>
        /// <param name="username">User Name</param>
        /// <param name="middlename">Middle Name</param>
        /// <param name="lastnamepat">Last name paternal</param>
        /// <param name="lastnamemat">last name maternal</param>
        /// <param name="email">email address</param>
        [Theory]
        [InlineData("Fredel", "Fredel", "Rey", "Pacheco", "Caamal", "fredel.pacheco@neoris.com")]
        public void Update_Administrator_Ok_Test(string nickname, string username, string middlename, string lastnamepat, string lastnamemat, string email)
        {
            var guid = context.OPERATOR.AsNoTracking().Where(w => w.USER_NAME == username).Select(s => s.OPERATOR_ID).FirstOrDefault();
            UpdateAdministratorsCommand command = new UpdateAdministratorsCommand()
            {
                UserId = guid.ToString(),
                NickName = nickname,
                UserName = username,
                LastNamePat = lastnamepat,
                LastNameMat = lastnamemat,
                MiddleName = middlename,
                Email = email,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            var getHandler = new UpdateAdministratorsHandler(logService.Object, context);
            var response = getHandler.Handle(command, new CancellationToken());

            Assert.True(response != null && response.IsCompleted && response.Result);
        }
    }
}

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
    /// Unit Test update administrator
    /// </summary>
    public class UpdateAdministratorsValidatorTest
    {
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<ICryptographyService> cryptoGraph;

        /// <summary>
        /// Constructor Class
        /// </summary>
        public UpdateAdministratorsValidatorTest()
        {
            mediator = new Mock<IMediator>();
            cryptoGraph = new Mock<ICryptographyService>();

            SetDataMock.EndPoint = "Administrators/Update";
            List<Domain.Entities.Device> lstDevice = SetDataMock.AddDevice();
            List<ExternalApplication> lstExternalApplication = SetDataMock.AddExternalApplication();
            List<SessionToken> lstSessionToken = SetDataMock.AddSessionToken();
            List<Workgroup> lstWorkgroup = SetDataMock.AddWorkgroup();
            List<UserWorkgroupLink> lstAddUserWorkgroupLink = SetDataMock.AddUserWorkgroupLink();
            List<WorkgroupPermissionStoreLink> lstWorkgroupPermissionStoreLink = SetDataMock.AddWorkgroupPermissionStoreLink();
            List<Module> lstModule = SetDataMock.AddModule();
            List<PermissionType> lstPermissionType = SetDataMock.AddPermissionsType();
            List<Permission> lstPermission = SetDataMock.AddPermissions();
            List<WorkgroupPermissionLink> lstWorkgroupPermissionLink = SetDataMock.AddWorkgroupPermissionLink();

            List<Person> lstPerson = SetDataMock.AddPerson();
            List<OperatorStatus> lstOperatorStatus = SetDataMock.AddStatusData();
            List<Operator> lstOperator = SetDataMock.AddOperator();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
              .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());

            context.DEVICE.AddRange(lstDevice);
            context.EXTERNAL_APPLICATION.AddRange(lstExternalApplication);
            context.WORKGROUP.AddRange(lstWorkgroup);
            context.USER_WORKGROUP_LINK.AddRange(lstAddUserWorkgroupLink);
            context.MODULE.AddRange(lstModule);
            context.PERMISSION_TYPE.AddRange(lstPermissionType);
            context.PERMISSION.AddRange(lstPermission);
            context.WORKGROUP_PERMISSION_LINK.AddRange(lstWorkgroupPermissionLink);
            context.WORKGROUP_PERMISSION_STORE_LINK.AddRange(lstWorkgroupPermissionStoreLink);
            context.SESSION_TOKEN.AddRange(lstSessionToken);

            context.PERSON.AddRange(lstPerson);
            context.OPERATOR_STATUS.AddRange(lstOperatorStatus);
            context.OPERATOR.AddRange(lstOperator);
            context.SaveChanges();
        }

        /// <summary>
        /// Validate if the inputs are correct
        /// </summary>
        /// <param name="nickname">this param is fake and your use is to get a guid</param>
        /// <param name="nickname">Nick Name</param>
        /// <param name="username">User Name</param>
        /// <param name="middlename">Middle Name</param>
        /// <param name="lastnamepat">Last name paternal</param>
        /// <param name="lastnamemat">last name maternal</param>
        /// <param name="email">email address</param>
        /// <param name="msgError">message error</param>
        //[Theory]
        //[InlineData("XXXXX", "Fredel", "Rey", "Pacheco", "Caamal", "fredel.pacheco@neoris.com", GlobalConstantMessages.USERIDINVALID)]
        //[InlineData("Fredel", "", "Rey", "Pacheco", "Caamal", "fredel.pacheco@neoris.com", GlobalConstantMessages.USERNAMEMENOTEMPTY)]
        //[InlineData("Fredel", "Fredel", "Reynel", "", "Caamal", "fredel.pacheco@neoris.com", GlobalConstantMessages.LASTNAMEPATNOTEMPTY)]
        //[InlineData("Fredel", "Fredel", "Reynel", "Pacheco", "Caamal", "", GlobalConstantMessages.EMAILNOTEMPTY)]

        //public async Task UpdateAdministratorsValidatorInput(string nickname, string username, string middlename, string lastnamepat, string lastnamemat, string email, string msgError)
        //{
        //    var guid = context.OPERATOR.AsNoTracking().Where(w => w.USER_NAME == nickname).Select(s => s.OPERATOR_ID).FirstOrDefault();

        //    UpdateAdministratorsCommandValidator commandValidator = new(context);
        //    UpdateAdministratorsCommand command = new()
        //    {
        //        UserId = guid.ToString() == Guid.Empty.ToString() ? string.Empty : guid.ToString(),
        //        NickName = nickname,
        //        UserName = username,
        //        LastNamePat = lastnamepat,
        //        LastNameMat = lastnamemat,
        //        MiddleName = middlename,
        //        Email = email,
        //        BearerToken = SetDataMock.Token
        //    };
        //    var commandResponse = await commandValidator.ValidateAsync(command);


        //    bool isValid = commandResponse.Errors.Any(a => a.ToString() == msgError);

        //    Assert.True(isValid);
        //}

        /// <summary>
        /// Validate if the email is correct
        /// </summary>
        /// <param name="nickname">this parameter is fake and your use is to get a guid</param>
        /// <param name="nickname">Nick Name</param>
        /// <param name="username">User Name</param>
        /// <param name="middlename">Middle Name</param>
        /// <param name="lastnamepat">Last name paternal</param>
        /// <param name="lastnamemat">last name maternal</param>
        /// <param name="email">email address</param>
        /// <param name="msgError">message error</param>
        //[Theory]
        //[InlineData("Fredel", "Fredel", "Reynel", "Pacheco", "Caamal", "fredel.pacheconeoris.com", GlobalConstantMessages.EMAILINVALID)]
        //[InlineData("Fredel", "Fredel", "Reynel", "Pacheco", "Caamal", "fredel.pacheco@neoris..co", GlobalConstantMessages.EMAILINVALID)]
        //[InlineData("Fredel", "Fredel", "Reynel", "Pacheco", "Caamal", "@fredel.pacheconeoris.com", GlobalConstantMessages.EMAILINVALID)]
        //[InlineData("Fredel", "Fredel", "Reynel", "Pacheco", "Caamal", "fredel.pacheconeoris.com@", GlobalConstantMessages.EMAILINVALID)]
        //public async Task UpdateAdministratorsValidatorEmail(string nickname, string username, string middlename, string lastnamepat, string lastnamemat, string email, string msgError)
        //{
        //    var guid = context.OPERATOR.AsNoTracking().Where(w => w.USER_NAME == nickname).Select(s => s.OPERATOR_ID).FirstOrDefault();

        //    UpdateAdministratorsCommandValidator commandValidator = new(context);
        //    UpdateAdministratorsCommand command = new()
        //    {
        //        UserId = guid.ToString() == Guid.Empty.ToString() ? string.Empty : guid.ToString(),
        //        NickName = nickname,
        //        UserName = username,
        //        LastNamePat = lastnamepat,
        //        LastNameMat = lastnamemat,
        //        MiddleName = middlename,
        //        Email = email,
        //        BearerToken = SetDataMock.Token
        //    };
        //    var commandResponse = await commandValidator.ValidateAsync(command);

        //    msgError = string.Format(msgError, email);

        //    bool isValid = commandResponse.Errors.Any(a => a.ToString() == msgError);

        //    Assert.True(isValid);
        //}
    }
}

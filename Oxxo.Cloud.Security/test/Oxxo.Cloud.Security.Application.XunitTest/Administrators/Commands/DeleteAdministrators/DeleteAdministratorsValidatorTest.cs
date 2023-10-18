#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
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

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.DeleteAdministrators
{
    /// <summary>
    /// Unit Test Dete administrator
    /// </summary>
    public class DeleteAdministratorsValidatorTest
    {
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<ICryptographyService> cryptoGraph;

        /// <summary>
        /// Constructor Class
        /// </summary>
        public DeleteAdministratorsValidatorTest()
        {
            mediator = new Mock<IMediator>();
            cryptoGraph = new Mock<ICryptographyService>();

            SetDataMock.EndPoint = "Administrators/Delete";
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
        /// Validate if de input id is correct
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="msgError">Message Error</param>
        //[Theory]
        //[InlineData(null, GlobalConstantMessages.USERIDINVALID)]
        //[InlineData("", GlobalConstantMessages.USERIDINVALID)]
        //[InlineData("Bad ID", GlobalConstantMessages.NORECORD)]
        //public async Task DeleteAdministratorsValidator_input(string userId, string msgError)
        //{
        //    DeleteAdministratorsCommandValidator commandValidator = new(context);
        //    DeleteAdministratorsCommand command = new DeleteAdministratorsCommand()
        //    {
        //        UserId = userId,
        //        BearerToken = SetDataMock.Token,
        //    };

        //    var commandResponse = await commandValidator.ValidateAsync(command);

        //    bool isValid = commandResponse.Errors.Any(a => a.ToString() == msgError);

        //    Assert.True(isValid);
        //}
    }
}

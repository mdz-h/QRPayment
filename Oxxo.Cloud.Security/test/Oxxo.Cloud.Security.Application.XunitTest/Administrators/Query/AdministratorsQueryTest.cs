#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    14/12/2022.
// Comment: Query Administrators.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Administrators.Query
{
    /// <summary>
    /// Test class to validate get administrators
    /// </summary>
    public class AdministratorsQueryTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;

        /// <summary>
        /// Constructor class
        /// </summary>
        public AdministratorsQueryTest()
        {
            logService = new Mock<ILogService>();
            mediator = new Mock<IMediator>();

            #region [INSERTS TO VALIDATE TOKEN]
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
            #endregion

            #region[ENTITY CONFIGURATIONS]
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
            #endregion
        }

        /// <summary>
        /// Validate if the method get information
        /// </summary>
        /// <param name="pagenumber">number of pages</param>
        /// <param name="itemnumber">number of items</param>
        /// <returns>List active administrators</returns>
        //[Theory]
        //[InlineData(0, 4)]
        //public void Get_ValidateInfo(int pagenumber, int itemnumber)
        //{
        //    AdministratorsQuery command = new()
        //    {
        //        PageNumber = pagenumber,
        //        ItemsNumber = itemnumber
        //    };

        //    var getHandler = new AdministratorsQueryHandler(context, logService.Object);
        //    var response = getHandler.Handle(command, new CancellationToken());

        //    Assert.True(response != null && response.IsCompleted);
        //}
    }
}

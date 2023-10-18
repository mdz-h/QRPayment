﻿#region File Information
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
    /// Request DTO Class
    /// </summary>
    public class AdministratorsQueryByIdTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;

        /// <summary>
        /// Constructor class
        /// </summary>
        public AdministratorsQueryByIdTest()
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
        /// <param name="username">User name ID</param>
        /// <returns>List active administrators</returns>
        //[Theory]
        //[InlineData("Fredel")]
        //public void GetAdministratorById_ValidateInfo(string userId)
        //{
        //    var guid = context.OPERATOR.AsNoTracking().Where(w => w.USER_NAME == userId).Select(s => s.OPERATOR_ID).FirstOrDefault();

        //    AdministratorsQueryById command = new()
        //    {
        //        UserId = guid.ToString(),
        //    };

        //    var getHandler = new AdministratorsQueryByIdHandler(context, logService.Object);
        //    var response = getHandler.Handle(command, new CancellationToken());

        //    Assert.True(response != null && response.IsCompleted);
        //}

        /// <summary>
        /// Validate if the method get information
        /// </summary>
        /// <param name="username">User name ID</param>
        /// <returns>Error</returns>
        //[Fact]
        //public async Task GetAdministratorById_ValidateNotInfo()
        //{
        //    AdministratorsQueryById command = new()
        //    {
        //        UserId = "NOEXISTS ID",
        //    };

        //    var getHandler = new AdministratorsQueryByIdHandler(context, logService.Object);

        //    CustomException exception = await Assert.ThrowsAsync<CustomException>(() => getHandler.Handle(command, new CancellationToken()));

        //    Assert.Equal(GlobalConstantMessages.NOTRECORD, exception.Message);
        //}
    }
}

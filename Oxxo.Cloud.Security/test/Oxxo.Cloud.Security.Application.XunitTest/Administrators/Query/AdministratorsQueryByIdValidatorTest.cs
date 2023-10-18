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
    /// Unit Test get administrator
    /// </summary>
    public class AdministratorsQueryByIdValidatorTest
    {
        private readonly Mock<ILogService> logService;
        private readonly Mock<ITokenGenerator> tokenGenerator;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<ICryptographyService> cryptoGraph;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public AdministratorsQueryByIdValidatorTest()
        {
            logService = new Mock<ILogService>();
            tokenGenerator = new Mock<ITokenGenerator>();
            mediator = new Mock<IMediator>();
            cryptoGraph = new Mock<ICryptographyService>();
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
        /// This test validate if the send fields are correct
        /// </summary>
        /// <param name="fieldvalidate"></param>
        /// <param name="pagenumber"></param>
        /// <param name="itemnumber"></param>
        /// <param name="maxValue"></param>
        /// <param name="msgError"></param>
        /// <returns></returns>
        //[Theory]
        //[InlineData("", GlobalConstantMessages.FIELDEMPTY)]
        //public async Task GetAdministratorById_Validate_Fields_Empty(string userId, string msgError)
        //{
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AdministratorsQueryByIdValidator commandValidator = new(auth, logService.Object, tokenGenerator.Object);
        //    AdministratorsQueryById command = new()
        //    {
        //        UserId = userId,
        //    };

        //    var commandResponse = await commandValidator.ValidateAsync(command);

        //    msgError = string.Format(msgError, "User Id");

        //    bool isValid = commandResponse.Errors.Any(a => a.ToString().ToUpper() == msgError.ToUpper());
        //    Assert.True(isValid);
        //}
    }
}

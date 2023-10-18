#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-12-13.
// Comment: Command Assigment Roles to User Validation Test.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandValidationTest
    {
        private readonly Mock<ILogService> logService;
        readonly ApplicationDbContext context;
        readonly Mock<IMediator> Mediator;
        private readonly Mock<ITokenGenerator> tokenGenerator;
        private readonly Mock<ICryptographyService> cryptoGraph;

        public AssignRoleToUserCommandValidationTest()
        {
            logService = new Mock<ILogService>();
            Mediator = new Mock<IMediator>();
            tokenGenerator = new Mock<ITokenGenerator>();
            cryptoGraph = new Mock<ICryptographyService>();

            SetDataMock.EndPoint = GlobalConstantHelpers.PATHASSINGROLETOUSER;

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
            context = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor());
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

        //[Theory]
        //[InlineData("", 5, GlobalConstantMessages.ASSIGMENTROLETOUSERWORKGORUPGUID)]
        //public async Task AssigmentRolestoUserValidGUIDempty(string GUID, int WorkgroupId, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssignRoleToUserCommandValidator assignrolestoUsercommandValidator =
        //        new AssignRoleToUserCommandValidator(context);
        //    AssignRoleToUserCommand assignRoleToUser = new AssignRoleToUserCommand()
        //    {
        //        Guid = GUID,
        //        WorkgroupId = WorkgroupId
        //    };

        //    var response = await assignrolestoUsercommandValidator.ValidateAsync(assignRoleToUser);
        //    bool isValid = response.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}


        //[Theory]
        //[InlineData("37e3cbc4-ba4b-4b5c-98d2-870b9bef5c0d", 5, GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTEXIST)]
        //public async Task AssigmentRolestoUserValidGUIDNotexistinDB(string GUID, int WorkgroupId, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssignRoleToUserCommandValidator assignrolestoUsercommandValidator =
        //        new AssignRoleToUserCommandValidator(context);
        //    AssignRoleToUserCommand assignRoleToUser = new AssignRoleToUserCommand()
        //    {
        //        Guid = GUID,
        //        WorkgroupId = WorkgroupId
        //    };

        //    var response = await assignrolestoUsercommandValidator.ValidateAsync(assignRoleToUser);
        //    bool isValid = response.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}

        //[Theory]
        //[InlineData("a283fbb9-5f7e-4d64-e3e6-08dadd1d9e4e", 0, GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTZERO)]
        //[InlineData("a283fbb9-5f7e-4d64-e3e6-08dadd1d9e4e", -2, GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTZERO)]

        //public async Task AssigmentRolestoUserValidWorkgroupGreaterThanOrEqualToZero(string GUID, int WorkgroupId, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssignRoleToUserCommandValidator assignrolestoUsercommandValidator =
        //        new AssignRoleToUserCommandValidator(context);
        //    AssignRoleToUserCommand assignRoleToUser = new AssignRoleToUserCommand()
        //    {
        //        Guid = GUID,
        //        WorkgroupId = WorkgroupId
        //    };

        //    var response = await assignrolestoUsercommandValidator.ValidateAsync(assignRoleToUser);
        //    bool isValid = response.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}

        //[Theory]
        //[InlineData("a283fbb9-5f7e-4d64-e3e6-08dadd1d9e4e", 3, GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTEXIST)]
        //[InlineData("a283fbb9-5f7e-4d64-e3e6-08dadd1d9e4e", 4, GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTEXIST)]

        //public async Task AssigmentRolestoUserValidWorkgroupnotExistinDB(string GUID, int WorkgroupId, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssignRoleToUserCommandValidator assignrolestoUsercommandValidator =
        //        new AssignRoleToUserCommandValidator(context);
        //    AssignRoleToUserCommand assignRoleToUser = new AssignRoleToUserCommand()
        //    {
        //        Guid = GUID,
        //        WorkgroupId = WorkgroupId
        //    };

        //    var response = await assignrolestoUsercommandValidator.ValidateAsync(assignRoleToUser);
        //    bool isValid = response.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}

        //[Theory]
        //[InlineData("a283fbb9-5f7e-4d64-e3e6-08dadd1d9e4e", 2, GlobalConstantMessages.ASSIGMENTWORKGROUPIDANDGUIDEXIST)]
        //[InlineData("16a0c66a-874f-483e-0c12-08dadd3463bc", 1, GlobalConstantMessages.ASSIGMENTWORKGROUPIDANDGUIDEXIST)]

        //public async Task AssigmentRolestoUserValidUserWorkgroupExistinDB(string GUID, int WorkgroupId, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssignRoleToUserCommandValidator assignrolestoUsercommandValidator =
        //        new AssignRoleToUserCommandValidator(context);
        //    AssignRoleToUserCommand assignRoleToUser = new AssignRoleToUserCommand()
        //    {
        //        Guid = GUID,
        //        WorkgroupId = WorkgroupId
        //    };

        //    var response = await assignrolestoUsercommandValidator.ValidateAsync(assignRoleToUser);
        //    bool isValid = response.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}
    }
}

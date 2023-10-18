#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-12-13.
// Comment: Command Assigment Permission to Role Validation Test.
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

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.AssigmentPermissionstorole
{
    public class AssigmentPermissionToRoleCommandValidationTest
    {
        private readonly Mock<ILogService> logService;
        readonly ApplicationDbContext context;
        readonly Mock<IMediator> Mediator;

        public AssigmentPermissionToRoleCommandValidationTest()
        {
            logService = new Mock<ILogService>();
            Mediator = new Mock<IMediator>();
          
            SetDataMock.EndPoint = GlobalConstantHelpers.PATHASSINGPERMISIONSTOROLE;

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

        public static IEnumerable<object[]> NewInsertData()
        {
            yield return new object[] { 1, new List<int> { 2, 3, 4, 5 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78" };
            yield return new object[] { 2, new List<int> { 2, 3, 4, 5 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78" };
        }

        //[Theory]
        //[MemberData(nameof(NewInsertData))]
        //public async Task AssigmentPermissionToRole_InsertnewAssigmentRoletoUser_ReturnTrue(int WorkgroupId, List<int> PermissionId, string token)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssigmentPermissionToRoleCommandValidator assigmentPermissionToRoleCommandValidator =
        //        new AssigmentPermissionToRoleCommandValidator(context);
        //    AssigmentPermissionToRoleCommand cmd = new AssigmentPermissionToRoleCommand()
        //    {
        //        WorkgroupId = WorkgroupId,
        //        PermissionId = PermissionId,
        //        BearerToken = token
        //    };

        //    var result = await assigmentPermissionToRoleCommandValidator.ValidateAsync(cmd);

        //    Assert.True(result.Errors.Count == 0);
        //}

        public static IEnumerable<object[]> ASSIGMENTWORKGROUPIDNOTZERO()
        {

            yield return new object[] { null, new List<int> { 2, 3, 4, 5 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78", GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTZERO };
            yield return new object[] { 0, new List<int> { 2, 3, 4, 5 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78", GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTZERO };
            yield return new object[] { -1, new List<int> { 2, 3, 4, 5 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78", GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTZERO };
        }
        //[Theory]
        //[MemberData(nameof(ASSIGMENTWORKGROUPIDNOTZERO))]
        //public async Task AssigmentPermissionToRole_ValidateassigmentworkgroupidnotZero(int WorkgroupId, List<int> PermissionId,
        //    string token, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssigmentPermissionToRoleCommandValidator assigmentPermissionToRoleCommandValidator =
        //        new AssigmentPermissionToRoleCommandValidator(context);
        //    AssigmentPermissionToRoleCommand cmd = new AssigmentPermissionToRoleCommand()
        //    {
        //        WorkgroupId = WorkgroupId,
        //        PermissionId = PermissionId,
        //        BearerToken = token
        //    };

        //    var result = await assigmentPermissionToRoleCommandValidator.ValidateAsync(cmd);
        //    bool isValid = result.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}

        public static IEnumerable<object[]> ASSIGMENTWORKGROUPIDNOTEXISTDATA()
        {
            yield return new object[] { 3, new List<int> { 2, 3, 4, 5 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78", GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTEXIST };

        }
        //[Theory]
        //[MemberData(nameof(ASSIGMENTWORKGROUPIDNOTEXISTDATA))]
        //public async Task AssigmentPermissionToRole_ValidateExistRecordinDBReturnTrue(int WorkgroupId, List<int> PermissionId,
        //    string token, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssigmentPermissionToRoleCommandValidator assigmentPermissionToRoleCommandValidator =
        //        new AssigmentPermissionToRoleCommandValidator(context);
        //    AssigmentPermissionToRoleCommand cmd = new AssigmentPermissionToRoleCommand()
        //    {
        //        WorkgroupId = WorkgroupId,
        //        PermissionId = PermissionId,
        //        BearerToken = token
        //    };

        //    var result = await assigmentPermissionToRoleCommandValidator.ValidateAsync(cmd);
        //    bool isValid = result.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}


        public static IEnumerable<object[]> ASSIGMENTPERMISSIONIDNOTZERODATA()
        {
            yield return new object[] { 1, new List<int> { 0, 3, 4, 5 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78", GlobalConstantMessages.ASSIGMENTPERMISSIONIDNOTZERO };
            yield return new object[] { 1, new List<int> { 1, -2, 4, 5 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78", GlobalConstantMessages.ASSIGMENTPERMISSIONIDNOTZERO };
        }

        //[Theory]
        //[MemberData(nameof(ASSIGMENTPERMISSIONIDNOTZERODATA))]
        //public async Task AssigmentPermissionToRole_ValidatePermissionIdGreaterThanOrEqualtoZero(int WorkgroupId, List<int> PermissionId,
        //   string token, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssigmentPermissionToRoleCommandValidator assigmentPermissionToRoleCommandValidator =
        //        new AssigmentPermissionToRoleCommandValidator(context);
        //    AssigmentPermissionToRoleCommand cmd = new AssigmentPermissionToRoleCommand()
        //    {
        //        WorkgroupId = WorkgroupId,
        //        PermissionId = PermissionId,
        //        BearerToken = token
        //    };

        //    var result = await assigmentPermissionToRoleCommandValidator.ValidateAsync(cmd);
        //    bool isValid = result.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}

        public static IEnumerable<object[]> ASSIGMENTPERMISSIONIDNOTEXISTDATA()
        {
            yield return new object[] { 1, new List<int> { 6, 7, 8, 9 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78", GlobalConstantMessages.ASSIGMENTPERMISSIONIDNOTEXIST };

        }

        //[Theory]
        //[MemberData(nameof(ASSIGMENTPERMISSIONIDNOTEXISTDATA))]
        //public async Task AssigmentPermissionToRole_ValidatePermissionIdNotexistinDB(int WorkgroupId, List<int> PermissionId,
        //   string token, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssigmentPermissionToRoleCommandValidator assigmentPermissionToRoleCommandValidator =
        //        new AssigmentPermissionToRoleCommandValidator(context);
        //    AssigmentPermissionToRoleCommand cmd = new AssigmentPermissionToRoleCommand()
        //    {
        //        WorkgroupId = WorkgroupId,
        //        PermissionId = PermissionId,
        //        BearerToken = token
        //    };

        //    var result = await assigmentPermissionToRoleCommandValidator.ValidateAsync(cmd);
        //    bool isValid = result.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}

        public static IEnumerable<object[]> PERMISSIONEXISTRECORDDATA()
        {
            yield return new object[] { 1, new List<int> { 1, 3, 5 }, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78", GlobalConstantMessages.PERMISSIONEXISTRECORD };

        }

        //[Theory]
        //[MemberData(nameof(PERMISSIONEXISTRECORDDATA))]
        //public async Task AssigmentPermissionToRole_ValidateexistRecordassigmenteroletouserinDB(int WorkgroupId, List<int> PermissionId,
        //   string token, string msgError)
        //{
        //    //Arrange
        //    AuthenticateQuery auth = new(context, cryptoGraph.Object);
        //    AssigmentPermissionToRoleCommandValidator assigmentPermissionToRoleCommandValidator =
        //        new AssigmentPermissionToRoleCommandValidator(context);
        //    AssigmentPermissionToRoleCommand cmd = new AssigmentPermissionToRoleCommand()
        //    {
        //        WorkgroupId = WorkgroupId,
        //        PermissionId = PermissionId,
        //        BearerToken = token
        //    };

        //    var result = await assigmentPermissionToRoleCommandValidator.ValidateAsync(cmd);
        //    bool isValid = result.Errors.Any(a => a.ToString() == msgError);
        //    Assert.True(isValid);
        //}
    }
}

#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    2023-01-04.
// Comment: Class Test of assign external application to role
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.AssignExternalAppsToRole
{
    /// <summary>
    /// Unit Test to validate  of assign external application to role
    /// </summary>
    public class AssignExternalAppsToRoleCommandValidatorTest
    {
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;

        /// <summary>
        /// Initial properties on constructor class
        /// </summary>
        public AssignExternalAppsToRoleCommandValidatorTest()
        {
            mediator = new Mock<IMediator>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
             .Options;

            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());
        }

        /// <summary>
        /// Validate the request parameters required
        /// </summary>
        /// <param name="externalapp">External Application ID</param>
        /// <param name="roleId">Role ID</param>
        /// <param name="fieldName">Field name to validate</param>
        /// <returns>Assert True</returns>
        [Theory]
        [InlineData("", 1, "External App Id")]
        [InlineData("XXXX-YYYY", 0, "Workgroup Id")]
        public async Task AssignExternalAppsToRoleValidator_Test(string externalapp, int roleId, string fieldName)
        {
            string msgError = GlobalConstantMessages.FIELDEMPTY;

            AssignExternalAppsToRoleCommandValidator validationRules = new(context);
            AssignExternalAppsToRoleCommand roleCommand = new()
            {
                WorkgroupId = roleId,
                ExternalAppId = externalapp
            };

            var commandResponse = await validationRules.ValidateAsync(roleCommand);

            msgError = string.Format(msgError, fieldName);
            bool isValid = commandResponse.Errors.Any(a => a.ToString().ToUpper() == msgError.ToUpper());

            Assert.True(isValid);
        }

        /// <summary>
        /// Validate if not exists external Application
        /// </summary>
        /// <param name="externalapp">External Application ID</param>
        /// <param name="roleId">Role ID</param>
        /// <returns>Assert True</returns>
        [Theory]
        [InlineData("XXXX-YYYY", 1)]
        public async Task AssignExternalAppsToRoleValidator_ValidateExternalApp_Test(string externalapp, int roleId)
        {
            AssignExternalAppsToRoleCommandValidator validationRules = new(context);
            AssignExternalAppsToRoleCommand roleCommand = new()
            {
                WorkgroupId = roleId,
                ExternalAppId = externalapp
            };

            bool isValid = await validationRules.ValidateExternalApp(roleCommand, new CancellationToken());

            Assert.True(!isValid);
        }

        /// <summary>
        /// Validate if not exists Role
        /// </summary>
        /// <param name="externalapp">External Application ID</param>
        /// <param name="roleId">Role ID</param>
        /// <returns>Assert True</returns>
        [Theory]
        [InlineData("XXXX-YYYY", 1)]
        public async Task AssignExternalAppsToRoleValidator_ValidateRole_Test(string externalapp, int roleId)
        {
            AssignExternalAppsToRoleCommandValidator validationRules = new(context);
            AssignExternalAppsToRoleCommand roleCommand = new()
            {
                WorkgroupId = roleId,
                ExternalAppId = externalapp
            };

            bool isValid = await validationRules.ValidateRole(roleCommand, new CancellationToken());

            Assert.True(!isValid);
        }

        /// <summary>
        /// Validate if not exists the relationship between external application and role
        /// </summary>
        /// <returns>Assert True</returns>
        [Fact]
        public async Task AssignExternalAppsToRoleValidator_ValidateRelationship_Test()
        { 
            AssignExternalAppsToRoleCommandValidator validationRules = new(context);
            AssignExternalAppsToRoleCommand roleCommand = new()
            {
                WorkgroupId = 1,
                ExternalAppId = SetDataMock.GuidSessionToken.ToString()
            };

            bool isValid = await validationRules.ValidateRelationship(roleCommand, new CancellationToken());

            Assert.True(isValid);
        }
    }
}

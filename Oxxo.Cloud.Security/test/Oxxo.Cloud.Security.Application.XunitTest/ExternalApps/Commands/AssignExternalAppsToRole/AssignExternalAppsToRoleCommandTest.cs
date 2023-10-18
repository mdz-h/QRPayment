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
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.AssignExternalAppsToRole
{
    /// <summary>
    /// Process to validate the assign external application to role
    /// </summary>
    public class AssignExternalAppsToRoleCommandTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> mediator;

        /// <summary>
        /// Initial properties on constructor class
        /// </summary>
        public AssignExternalAppsToRoleCommandTest()
        {
            logService = new Mock<ILogService>();
            mediator = new Mock<IMediator>();


            List<Workgroup> workgroups = SetDataMock.AddWorkgroup();
            List<ExternalApplication> externalapplications = SetDataMock.AddExternalApplication();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
             .Options;
            context = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());

            context.WORKGROUP.AddRange(workgroups);
            context.EXTERNAL_APPLICATION.AddRange(externalapplications);
            context.SaveChanges();
        }

        /// <summary>
        /// Validate if the process to create the relationship between external application and role are correct
        /// </summary>
        /// <returns>Assert true</returns>
        [Fact]
        public void AssignExternalAppsToRoleHandle_Test()
        {
            AssignExternalAppsToRoleCommand command = new()
            {
                ExternalAppId = SetDataMock.GuidSessionToken.ToString(),
                WorkgroupId = 1,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            var getHandler = new AssignExternalAppsToRoleHandle(logService.Object, context);
            var response = getHandler.Handle(command, new CancellationToken());

            Assert.True(response != null && response.IsCompleted && response.Result);
        }
    }
}

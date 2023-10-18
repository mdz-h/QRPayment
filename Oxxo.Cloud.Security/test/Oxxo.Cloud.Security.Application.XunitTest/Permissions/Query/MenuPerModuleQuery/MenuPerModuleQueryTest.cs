#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristáin (NEORIS).
// Date:    08/05/2023.
// Comment: Class Get Menu of Front End Test.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Permissions.Queries.MenuPerModuleQuery;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Permissions.Query.MenuPerModuleQueryTest
{
    public class MenuPerModuleQueryTest
    {
        private readonly Mock<IMediator> mediator;
        private readonly ApplicationDbContext applicationDbContextFake;
        private readonly Mock<ILogService> log;

        public MenuPerModuleQueryTest()
        {
            mediator = new Mock<IMediator>();
            log = new Mock<ILogService>();

            List<PermissionType> PERMISSION_TYPE = new()
            {
                new()
                {
                    PERMISSION_TYPE_ID = 1,
                    CODE = "ENDPOINT",
                    IS_ACTIVE = true
                },
                new()
                {
                    PERMISSION_TYPE_ID = 2,
                    CODE = "FRONTEND",
                    IS_ACTIVE = true
                }
            };

            List<Domain.Entities.Module> MODULE = new()
            {
                new()
                {
                    MODULE_ID = 1,
                    NAME = "OPERATING_INVENTORY",
                    IS_ACTIVE = true
                },
                new()
                {
                    MODULE_ID = 2,
                    NAME = "XPOS",
                    IS_ACTIVE = true
                }
            };

            List<Permission> PERMISSION = new()
            {
                new Permission()
                {
                    PERMISSION_ID = 1,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 2,
                    CODE = "TEST1",
                    IS_ACTIVE = true
                },
                new Permission()
                {
                    PERMISSION_ID = 2,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 2,
                    CODE = "TEST2",
                    IS_ACTIVE = true
                },
                new Permission()
                {
                    PERMISSION_ID = 3,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 2,
                    CODE = "TEST3",
                    IS_ACTIVE = true
                },
                new Permission()
                {
                    PERMISSION_ID = 4,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 2,
                    CODE = "TEST4",
                    IS_ACTIVE = true
                },
                new Permission()
                {
                    PERMISSION_ID = 5,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 2,
                    CODE = "TEST5",
                    IS_ACTIVE = true
                },
                new Permission()
                {
                    PERMISSION_ID = 6,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 2,
                    CODE = "TEST6",
                    IS_ACTIVE = true
                },
                new Permission()
                {
                    PERMISSION_ID = 7,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 2,
                    CODE = "TEST7",
                    IS_ACTIVE = true
                },
                new Permission()
                {
                    PERMISSION_ID = 8,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 2,
                    CODE = "TEST8",
                    IS_ACTIVE = true
                },
                new Permission()
                {
                    PERMISSION_ID = 9,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 2,
                    CODE = "TEST9",
                    IS_ACTIVE = true
                },
            };

            List<PermissionFrontEnd> PERMISSION_FRONTEND = new()
            {
                new()
                {
                    PERMISSION_FRONTEND_ID = 1,
                    PERMISSION_ID = 1,
                    ICON = "main.icon",
                    PARENT_ID = 0,
                    SORT_ORDER = 0,
                    QUICK_ACCESS = null,
                    FL= true
                },
                new()
                {
                    PERMISSION_FRONTEND_ID = 2,
                    PERMISSION_ID = 2,
                    ICON= "inv.icon",
                    PARENT_ID = 0,
                    SORT_ORDER = 0,
                    QUICK_ACCESS = null,
                    FL= true
                },
                new()
                {
                    PERMISSION_FRONTEND_ID = 3,
                    PERMISSION_ID = 3,
                    ICON= "init.icon",
                    PARENT_ID = 0,
                    SORT_ORDER = 1,
                    QUICK_ACCESS = "F3",
                    FL= true
                },
                new()
                {
                    PERMISSION_FRONTEND_ID = 4,
                    PERMISSION_ID = 4,
                    ICON = "ciclic.icon",
                    PARENT_ID = 0,
                    SORT_ORDER = 2,
                    QUICK_ACCESS = "F4",
                    FL= true
                },
                new()
                {
                    PERMISSION_FRONTEND_ID = 5,
                    PERMISSION_ID = 5,
                    ICON = "register.icon",
                    PARENT_ID = 2,
                    SORT_ORDER = 3,
                    QUICK_ACCESS = "F5",
                    FL= true
                },
                new()
                {
                    PERMISSION_FRONTEND_ID = 6,
                    PERMISSION_ID = 6,
                    ICON = "mtto.icon",
                    PARENT_ID = 2,
                    SORT_ORDER = 4,
                    QUICK_ACCESS = "F6",
                    FL= true
                },
                new()
                {
                    PERMISSION_FRONTEND_ID = 7,
                    PERMISSION_ID = 7,
                    ICON = "apreli.icon",
                    PARENT_ID = 2,
                    SORT_ORDER = 5,
                    QUICK_ACCESS = "F7",
                    FL= true
                },
                new()
                {
                    PERMISSION_FRONTEND_ID = 8,
                    PERMISSION_ID = 8,
                    ICON = "afectation.icon",
                    PARENT_ID = 2,
                    SORT_ORDER = 6,
                    QUICK_ACCESS = "F8",
                    FL= true
                },
                new()
                {
                    PERMISSION_FRONTEND_ID = 9,
                    PERMISSION_ID = 9,
                    ICON = "config.icon",
                    PARENT_ID = 2,
                    SORT_ORDER = 7,
                    QUICK_ACCESS = "F9",
                    FL= true
                }
            };

            List<WorkgroupPermissionLink> WORKGROUP_PERMISSION_LINK = new()
            {
                new()
                {
                    WORKGROUP_PERMISSION_LINK_ID = 1,
                    PERMISSION_ID = 1,
                    WORKGROUP_ID = 4,
                    IS_ACTIVE = true                    
                },
                new()
                {
                    WORKGROUP_PERMISSION_LINK_ID = 2,
                    PERMISSION_ID = 2,
                    WORKGROUP_ID = 4,
                    IS_ACTIVE = true                    
                },
                new()
                {
                    WORKGROUP_PERMISSION_LINK_ID = 3,
                    PERMISSION_ID = 3,
                    WORKGROUP_ID = 4,
                    IS_ACTIVE = true                    
                },
                new()
                {
                    WORKGROUP_PERMISSION_LINK_ID = 4,
                    PERMISSION_ID = 4,
                    WORKGROUP_ID = 4,
                    IS_ACTIVE = true                    
                },
                new()
                {
                    WORKGROUP_PERMISSION_LINK_ID = 5,
                    PERMISSION_ID = 5,
                    WORKGROUP_ID = 4,
                    IS_ACTIVE = true                    
                },
                new()
                {
                    WORKGROUP_PERMISSION_LINK_ID = 6,
                    PERMISSION_ID = 6,
                    WORKGROUP_ID = 4,
                    IS_ACTIVE = true                    
                },
                new()
                {
                    WORKGROUP_PERMISSION_LINK_ID = 7,
                    PERMISSION_ID = 7,
                    WORKGROUP_ID = 4,
                    IS_ACTIVE = true                    
                },
                new()
                {
                    WORKGROUP_PERMISSION_LINK_ID = 8,
                    PERMISSION_ID = 8,
                    WORKGROUP_ID = 4,
                    IS_ACTIVE = true                    
                },
                new()
                {
                    WORKGROUP_PERMISSION_LINK_ID = 9,
                    PERMISSION_ID = 9,
                    WORKGROUP_ID = 4,
                    IS_ACTIVE = true
                },
            };

            List<UserWorkgroupLink> USER_WORKGROUP_LINK = new()
            {
                new()
                {
                    USER_WORKGROUP_LINK_ID = 1,
                    WORKGROUP_ID = 4,
                    GUID = Guid.Parse("9EECAC09-FD19-428A-B98B-B9922D8A3EFA"),
                    IS_ACTIVE = true,
                }
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            applicationDbContextFake = new ApplicationDbContext(options, mediator.Object, new AuditableEntitySaveChangesInterceptor());
            applicationDbContextFake.PERMISSION_TYPE.AddRange(PERMISSION_TYPE);
            applicationDbContextFake.MODULE.AddRange(MODULE);            
            applicationDbContextFake.PERMISSION_FRONTEND.AddRange(PERMISSION_FRONTEND);
            applicationDbContextFake.WORKGROUP_PERMISSION_LINK.AddRange(WORKGROUP_PERMISSION_LINK);
            applicationDbContextFake.USER_WORKGROUP_LINK.AddRange(USER_WORKGROUP_LINK);
            applicationDbContextFake.PERMISSION.AddRange(PERMISSION);
            applicationDbContextFake.SaveChanges();            
        }
        
        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWlPWFhPIE5ldyIsInN1YiI6Im1pT1hYTyIsIklkIjoiNTkyMzFhM2YtMTU1Yy00MDY0LTgyOTMtZmYyOTk3MzVhNmU0IiwiZXhwIjoxNzM4NzUwMDAyLCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.1zTJWjip02yxPrIRJCz8P7ZadeZRm7YDev5-T22BaOY"
            , "OPERATING_INVENTORY", "9EECAC09-FD19-428A-B98B-B9922D8A3EFA")]
        public void MenuPerModuleQueryHandler_InputRequest_ReturnMenuFrontEndResponse(string token, string moduleName, string userId)
        {
            //Arrange 
            MenuPerModuleQuery request = new() { BearerToken = token,  ModuleName = moduleName, UserId = Guid.Parse(userId) };

            //Act
            var menuPerModuleQueryHandler = new MenuPerModuleQueryHandler(applicationDbContextFake, log.Object);

            var result = menuPerModuleQueryHandler.Handle(request, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);            
            Assert.Equal(2, result.Result[1].PERMISSION_ID);
            Assert.Equal(9, result.Result.Count);

        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWlPWFhPIE5ldyIsInN1YiI6Im1pT1hYTyIsIklkIjoiNTkyMzFhM2YtMTU1Yy00MDY0LTgyOTMtZmYyOTk3MzVhNmU0IiwiZXhwIjoxNzM4NzUwMDAyLCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.1zTJWjip02yxPrIRJCz8P7ZadeZRm7YDev5-T22BaOY"
    , "TEST_MODULE_NOT_EXISTS", "9EECAC09-FD19-428A-9999-B9922D8A3EFA")]
        public void MenuPerModuleQueryHandler_InputRequest_ReturnError(string token, string moduleName, string userId)
        {
            //Arrange 
            MenuPerModuleQuery request = new() { BearerToken = token, ModuleName = moduleName, UserId = Guid.Parse(userId) };

            //Act
            var menuPerModuleQueryHandler = new MenuPerModuleQueryHandler(applicationDbContextFake, log.Object);

            var result = menuPerModuleQueryHandler.Handle(request, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.NotNull(result!.Exception!.InnerException!.Message);           

        }
    }
}
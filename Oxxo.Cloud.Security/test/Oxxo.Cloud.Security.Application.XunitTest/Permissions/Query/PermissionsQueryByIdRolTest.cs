#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    01/12/2022.
// Comment: permissions.
//===============================================================================
#endregion

using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Permissions.Queries;

namespace Oxxo.Cloud.Security.Application.Permissions.Query
{
    public class PermissionsQueryByIdRolTest
    {
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public PermissionsQueryByIdRolTest()
        {
            this.context = new Mock<IApplicationDbContext>();
            this.logService = new Mock<ILogService>();
        }

        /// <summary>
        /// validate if the class handler not return null
        /// </summary>
        /// <param name="rolId">Role Id</param>
        [Theory]
        [InlineData(11)]
        public void GetPermissionByIdRol_PermissionsCommand_ValidateHandle_ReturnBool(int rolId)
        {

            PermissionsQueryByIdRol command = new()
            {
                roleId = rolId,
            };

            var handler = new PermissionsQueryByIdRolHandler(context.Object, logService.Object);

            //Act
            var result = handler.Handle(command, new CancellationToken());

            //Asert
            Assert.NotNull(result);
        }
    }
}

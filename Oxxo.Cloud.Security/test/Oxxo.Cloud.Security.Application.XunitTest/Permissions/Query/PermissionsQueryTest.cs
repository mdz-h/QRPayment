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
    public class PermissionsQueryTest
    {
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<ILogService> logService;

        /// <summary>
        /// Constructor Class
        /// </summary> 
        public PermissionsQueryTest()
        {
            this.context = new Mock<IApplicationDbContext>();
            this.logService = new Mock<ILogService>();
        }

        /// <summary>
        /// validate if the class handler not return null
        /// </summary>
        /// <param name="skip">field skip</param>
        /// <param name="take">field take</param>
        [Theory]
        [InlineData(11, 14)]
        public void Get_PermissionsCommand_ValidateHandle_ReturnBool(int skip, int take)
        {

            PermissionsQuery command = new()
            {
                PageNumber = skip,
                ItemsNumber = take,
            };

            var handler = new PermissionsQueryHandler(context.Object, logService.Object);

            //Act
            var result = handler.Handle(command, new CancellationToken());

            //Asert
            Assert.NotNull(result);
        }
    }
}

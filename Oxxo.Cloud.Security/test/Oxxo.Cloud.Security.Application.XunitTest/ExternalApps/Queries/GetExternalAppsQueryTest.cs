#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-14.
// Comment: Class Test of External Apps Query Handler.
//===============================================================================
#endregion

using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.ExternalApps.Queries.Get;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Queries
{
    public class GetExternalAppsQueryTest
    {
        #region Properties
        /// <summary>
        /// Contract Mock of LogService
        /// </summary>
        private readonly Mock<ILogService> _logService;

        /// <summary>
        /// Contract Mock of ExternalAppsQueryGet
        /// </summary>
        private readonly Mock<IExternalAppsQueryGet> _externalAppsQueryGet;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GetExternalAppsQueryTest()
        {
            _logService = new Mock<ILogService>();
            _externalAppsQueryGet = new Mock<IExternalAppsQueryGet>();
        }
        #endregion
        #region Test methods
        /// <summary>
        /// Test method Handler
        /// </summary>
        /// <param name="identifier">Id</param>
        /// <param name="itemsNumber">Item Number</param>
        /// <param name="pagenumber">Page Number</param>
        [Theory]
        [InlineData("1234", 10, 1)]
        [InlineData("", 0, 0)]
        [InlineData(null, null, null)]
        public void HanldeShould(string identifier, int itemsNumber, int pagenumber)
        {
            //Arrange            
            GetExternalAppsQuery request = new GetExternalAppsQuery()
            {
                Identifier = identifier,
                ItemsNumber = itemsNumber,
                PageNumber = pagenumber
            };
            var handler = new GetExternalAppsHandler(_logService.Object, _externalAppsQueryGet.Object);

            //Act
            var response = handler.Handle(request, new CancellationToken());

            //Assert
            Assert.NotNull(response);
        }
        #endregion
    }
}

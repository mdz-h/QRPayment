#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-14.
// Comment: Class Test of External Apps Query Get.
//===============================================================================
#endregion

using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.ExternalApps.Queries.Get;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Queries
{
    public class ExternalAppsQueryGetTest
    {
        #region Properties
        /// <summary>
        /// Contract mock of ApplicationDbContext
        /// </summary>
        private readonly Mock<IApplicationDbContext> applicationDbContext;
        private readonly Mock<ICryptographyService> cryptoService;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ExternalAppsQueryGetTest()
        {
            applicationDbContext = new Mock<IApplicationDbContext>();
            cryptoService = new Mock<ICryptographyService>();
        }
        #endregion
        #region Test methods
        /// <summary>
        /// Test Method Get
        /// </summary>
        /// <param name="identifier">Id</param>
        /// <param name="itemsNumber">Items Number</param>
        /// <param name="pagenumber">Page Number</param>
        [Theory]
        [InlineData("1234", 10, 1)]
        [InlineData("", 0, 0)]
        public void GetShould(string identifier, int itemsNumber, int pagenumber)
        {
            //Arrange            
            GetExternalAppsQuery request = new()
            {
                Identifier = identifier,
                ItemsNumber = itemsNumber,
                PageNumber = pagenumber
            };
            ExternalAppsQueryGet get = new(applicationDbContext.Object, cryptoService.Object);

            //Act
            var response = get.Get(request);

            //Assert
            Assert.NotNull(response);
        }
        #endregion
    }
}
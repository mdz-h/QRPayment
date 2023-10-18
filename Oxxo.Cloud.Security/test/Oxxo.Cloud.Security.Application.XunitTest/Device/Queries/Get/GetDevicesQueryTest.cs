#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-11-26.
// Comment: Class for unit test of GetDevicesQuery.cs
//===============================================================================
#endregion
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;

namespace Oxxo.Cloud.Security.Application.Device.Queries.Get
{
    public class GetDevicesQueryTest
    {
        private Mock<ILogService> logService;
        private Mock<IDeviceQueryGet> deviceQuery;

        public GetDevicesQueryTest()
        {
            this.logService = new Mock<ILogService>();
            this.deviceQuery = new Mock<IDeviceQueryGet>();
        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78"
            , "Devices/Devices", 10, 1, "F1E12D08-EC6C-ED11-ADE5-A04A5E6D758C")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78"
            , "Devices/Devices", 10, 1, "")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78"
            , "Devices/Devices", 0, 0, "")]
        [InlineData("1234", "", -20, -10, "")]
        public void GetDevicesQuery_InputRequest_ReturnNotNullIsCompletedAndValidateHeader(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {

            //Arrange            
            GetDevicesQuery queryable = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            //Act
            var getDevicesHandler = new GetDevicesHandler(logService.Object, deviceQuery.Object);
            var response = getDevicesHandler.Handle(queryable, new CancellationToken());
            var inputToken = queryable.Token;

            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsCompleted);
            Assert.Equal(token, inputToken);

        }

    }
}

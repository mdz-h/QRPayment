#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-11-26.
// Comment: Class for unit test of GetDevicesQueryValidator.cs
//===============================================================================
#endregion
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using System.Reflection;

namespace Oxxo.Cloud.Security.Application.Device.Queries.Get
{
    public class GetDevicesQueryValidatorTest
    {
        private Mock<IDeviceQueryGet> deviceQuery;

        public GetDevicesQueryValidatorTest()
        {
            deviceQuery = new Mock<IDeviceQueryGet>();
        }

        [Fact]
        public void GetDevicesQueryValidator_ValidateConstructor()
        {
            //Act  
            GetDevicesQueryValidator getDevicesQueryValidator = new GetDevicesQueryValidator(deviceQuery.Object);
            //Assert
            Assert.NotNull(getDevicesQueryValidator);
        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78"
            , "Devices/Devices", 10, 1, "F1E12D08-EC6C-ED11-ADE5-A04A5E6D758C")]
        public void ValidateTypesParams_InputRequest_validateNotNullAndParams(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {

            // Arrange
            GetDevicesQuery request = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            Type type = typeof(GetDevicesQueryValidator);
            var getDevicesQueryValidator = Activator.CreateInstance(type, deviceQuery.Object);

            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "ValidateParamsTypes" && x.IsPrivate).First();

            //Act
            var ValidateTypesParams = method.Invoke(getDevicesQueryValidator, new object[] { request });

            //Assert
            Assert.NotNull(ValidateTypesParams);

        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78"
    , "Devices/Devices", 10, 1, "F1E12D08-EC6C-ED11-ADE5-A04A5E6D758C")]
        public void ValidateIfExistsDevices_InputRequest_validateNotNullAndParams(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {

            // Arrange
            GetDevicesQuery request = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            Type type = typeof(GetDevicesQueryValidator);
            var getDevicesQueryValidator = Activator.CreateInstance(type, deviceQuery.Object);

            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "ValidateIfExistsDevices" && x.IsPrivate).First();

            //Act
            var validateIfExistsDevices = method.Invoke(getDevicesQueryValidator, new object[] { request, new CancellationToken() });

        }
    }
}
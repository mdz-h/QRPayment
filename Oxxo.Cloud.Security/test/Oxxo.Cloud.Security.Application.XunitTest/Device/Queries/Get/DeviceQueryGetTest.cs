#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-11-26.
// Comment: Class for unit test of DeviceQueryGet.cs
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Device.Queries.Get
{
    public class DeviceQueryGetTest
    {
        readonly Mock<IMediator> Mediator;
        readonly ApplicationDbContext ApplicationDbContextFake;

        public DeviceQueryGetTest()
        {
            Mediator = new Mock<IMediator>();

            List<StorePlace> PLACES = new List<StorePlace>();
            PLACES.Add(new StorePlace()
            {
                STORE_PLACE_ID = 1,
                CR_STORE = "PLACE_TEST",
                CR_PLACE = "CR_PLACE_TEST",
                IS_ACTIVE = true
            });

            List<DeviceNumber> DEVIDES_NUMBER = new List<DeviceNumber>();
            DEVIDES_NUMBER.Add(new DeviceNumber()
            {
                DEVICE_NUMBER_ID = 1,
                NUMBER = 1,
                IS_ACTIVE = true
            });

            List<DeviceStatus> DEVICE_STATUS = new List<DeviceStatus>();
            DEVICE_STATUS.Add(new DeviceStatus()
            {
                DEVICE_STATUS_ID = 1,
                CODE = "OPEN",
                NAME = "OPEN",
                DESCRIPTION = "OPEN",
                IS_ACTIVE = true
            });

            List<DeviceType> DEVICE_TYPE = new List<DeviceType>();
            DEVICE_TYPE.Add(new DeviceType()
            {
                DEVICE_TYPE_ID = 1,
                CODE = "DEVICE",
                NAME = "DEVICE",
                DESCRIPTION = "DEVICE",
                IS_ACTIVE = true
            });


            List<Domain.Entities.Device> DEVICES = new List<Domain.Entities.Device>();
            for (var count = 0; count <= 100; count++)
            {
                var DeviceId = Guid.Empty;
                if (count == 0) DeviceId = new Guid("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c");
                if (count == 1) DeviceId = new Guid("693aaaee-256c-ed11-ade5-a04a5e6d758c");
                DEVICES.Add(new Domain.Entities.Device()
                {
                    DEVICE_ID = DeviceId,
                    NAME = $"NAME_TEST {count}",
                    MAC_ADDRESS = $"MAC_TEST {count}",
                    IP = $"IP_TEST {count}",
                    STORE_PLACE_ID = 1,
                    DEVICE_NUMBER_ID = 1,
                    DEVICE_STATUS_ID = 1,
                    DEVICE_TYPE_ID = 1,
                    PROCESSOR = $"PROCESSOR_TEST {count}",
                    NETWORK_CARD = $"NETWORK_CARD_TEST {count}",
                    IS_SERVER = true,
                    IS_ACTIVE = true,
                    CREATED_DATETIME = DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    MODIFIED_DATETIME = null,
                    MODIFIED_BY_OPERATOR_ID = null,
                    LCOUNT = 1
                });
            }
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            ApplicationDbContextFake = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor());
            ApplicationDbContextFake.STORE_PLACE.AddRange(PLACES);
            ApplicationDbContextFake.DEVICE_NUMBER.AddRange(DEVIDES_NUMBER);
            ApplicationDbContextFake.DEVICE.AddRange(DEVICES);
            ApplicationDbContextFake.DEVICE_STATUS.AddRange(DEVICE_STATUS);
            ApplicationDbContextFake.DEVICE_TYPE.AddRange(DEVICE_TYPE);
            ApplicationDbContextFake.SaveChanges();
        }




        [Theory]
        [InlineData("Token_TEST", "Endpoint_TEST", 10, 1, "f1e12d08-ec6c-ed11-ade5-a04a5e6d758c")]
        public void DeviceQueryGetGetDevices_InputRequest_ReturnListDeviceResponseCountOne(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {
            //Arrange 
            GetDevicesQuery request = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            //Act
            var geviceQueryGet = new DeviceQueryGet(ApplicationDbContextFake);
            var result = geviceQueryGet.GetDevices(request);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.Single(result.Result);
            Assert.Equal("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", result.Result[0].Device_id.ToString());
        }

        [Theory]
        [InlineData("Token_TEST", "Endpoint_TEST", 10, 1, "")]
        public void DeviceQueryGetGetDevices_InputRequest_ReturnListDeviceResponseCountTen(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {
            //Arrange 
            GetDevicesQuery request = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            //Act
            var geviceQueryGet = new DeviceQueryGet(ApplicationDbContextFake);
            var result = geviceQueryGet.GetDevices(request);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.Equal(10, result.Result.Count);
            Assert.Equal("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", result.Result[0].Device_id.ToString());
            Assert.Equal("693aaaee-256c-ed11-ade5-a04a5e6d758c", result.Result[1].Device_id.ToString());
        }

        [Theory]
        [InlineData("Token_TEST", "Endpoint_TEST", 5, 1, "")]
        public void DeviceQueryGetGetDevices_InputRequest_ReturnListDeviceResponseCountFive(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {
            //Arrange 
            GetDevicesQuery request = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            //Act
            var geviceQueryGet = new DeviceQueryGet(ApplicationDbContextFake);
            var result = geviceQueryGet.GetDevices(request);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.Equal(5, result.Result.Count);
            Assert.Equal("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", result.Result[0].Device_id.ToString());
            Assert.Equal("693aaaee-256c-ed11-ade5-a04a5e6d758c", result.Result[1].Device_id.ToString());
        }

        [Theory]
        [InlineData("Token_TEST", "Endpoint_TEST", 20, 2, "")]
        public void DeviceQueryGetGetDevices_InputRequest_ReturnListDeviceResponseCounttwentyPageTwo(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {
            //Arrange 
            GetDevicesQuery request = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            //Act
            var geviceQueryGet = new DeviceQueryGet(ApplicationDbContextFake);
            var result = geviceQueryGet.GetDevices(request);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.Equal(20, result.Result.Count);
            Assert.NotEqual("f1e12d08-ec6c-ed11-ade5-a04a5e6d758c", result.Result[0].Device_id.ToString());
            Assert.NotEqual("693aaaee-256c-ed11-ade5-a04a5e6d758c", result.Result[1].Device_id.ToString());
        }

        [Theory]
        [InlineData("", "", 3000, 10000, "")]
        [InlineData("", "", -2, -1, "")]
        public void DeviceQueryGetGetDevices_InputRequest_ReturnListCountZero(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {
            //Arrange 
            GetDevicesQuery request = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            //Act
            var geviceQueryGet = new DeviceQueryGet(ApplicationDbContextFake);
            var result = geviceQueryGet.GetDevices(request);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.Empty(result.Result);
        }

        [Theory]
        [InlineData("Token_TEST", "Endpoint_TEST", 10, 1, "f1e12d08-ec6c-ed11-ade5-a04a5e6d758c")]
        [InlineData("Token_TEST", "Endpoint_TEST", 10, 1, "")]
        [InlineData("Token_TEST", "Endpoint_TEST", 20, 2, "")]
        public void DeviceQueryGetValidateIfExistsDevices_InputRequest_ReturnTrue(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {
            //Arrange 
            GetDevicesQuery request = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            //Act
            var geviceQueryGet = new DeviceQueryGet(ApplicationDbContextFake);
            var result = geviceQueryGet.ValidateIfExistsDevices(request);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.True(result.Result);
        }

        [Theory]
        [InlineData("Token_TEST", "Endpoint_TEST", 10, 1, "f1e12d08-ec6c-ed11-ade5-999999999")]
        [InlineData("Token_TEST", "Endpoint_TEST", -1, -41, "f1e12d08-ec6c-ed11-ade5-999999999")]
        [InlineData("Token_TEST", "Endpoint_TEST", 200000, 411232442, "f1e12d08-ec6c-ed11-ade5-999999999")]
        public void DeviceQueryValidateIfExistsDevices_InputRequest_ReturnFalse(string token, string endPoint, int itemsNumber, int pagenumber, string deviceIdentifier)
        {
            //Arrange 
            GetDevicesQuery request = new() { BearerToken = token, EndPoint = endPoint, ItemsNumber = itemsNumber, PageNumber = pagenumber, DeviceIdentifier = deviceIdentifier };

            //Act
            var geviceQueryGet = new DeviceQueryGet(ApplicationDbContextFake);
            var result = geviceQueryGet.ValidateIfExistsDevices(request);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.Result);
        }

    }
}

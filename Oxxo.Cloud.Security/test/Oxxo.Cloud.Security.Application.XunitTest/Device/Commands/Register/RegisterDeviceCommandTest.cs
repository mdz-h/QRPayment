using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;

namespace Oxxo.Cloud.Security.Application.Device.Commands.Register
{
    public class RegisterDeviceCommandTest
    {
        private readonly Mock<ILogService> logService;
        readonly ApplicationDbContext context;
        readonly Mock<IMediator> Mediator;

        public RegisterDeviceCommandTest()
        {
            logService = new Mock<ILogService>();
            Mediator = new Mock<IMediator>();
            List<StorePlace> PLACES = AddStorePlaceMockData();
            List<DeviceNumber> DEVICES_NUMBER = AddDeviceNumberMockData();
            List<DeviceType> DEVICES_TYPE = AddDeviceTypeMockData();
            List<DeviceStatus> DEVICE_STATUS = AddDeviceStatusMockData();
            List<ValidIPRange> VALID_IP_RANGE = AddValidIPRangeMockData();
            Workgroup WORKGROUP = AddDefaultWorkgroupData();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            context = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.STORE_PLACE.AddRange(PLACES);
            context.DEVICE_NUMBER.AddRange(DEVICES_NUMBER);
            context.DEVICE_TYPE.AddRange(DEVICES_TYPE);
            context.DEVICE_STATUS.AddRange(DEVICE_STATUS);
            context.VALID_IP_RANGE.AddRange(VALID_IP_RANGE);
            context.WORKGROUP.AddRange(WORKGROUP);
            context.SaveChanges();
        }

        [Theory]
        [InlineData("MAN9YKPRO", "10MAN", "509YK", 1, "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("MAN9YKOMAR", "10MAN", "50D49", 1, "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("MAN9YKPRO", "10MAN", "507VH", 3, "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public void RegisterDevice_InputDataDevice_ReturnNotNullIsCompletedTrue(string deviceName, string crPlace, string crStore,
            int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange      
            RegisterDeviceCommand registerDeviceCommand = new()
            {
                DeviceName = deviceName,
                CrPlace = crPlace,
                CrStore = crStore,
                TillNumber = tillNumber,
                MacAddress = macAddress,
                IP = ip,
                Processor = processor,
                NetworkCard = networkCard,
                IsServer = isServer,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            //Act
            var getDevicesHandler = new RegisterDeviceHandler(logService.Object, context);
            var response = getDevicesHandler.Handle(registerDeviceCommand, new CancellationToken());


            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsCompleted);
            Assert.True(response.Result);
        }

        [Theory]
        [InlineData("MAN9YKPRO", "10MAN", "509YK", 1, "TILL", "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("MAN9YKOMAR", "10MAN", "50D49", 1, "DEVICE", "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("MAN9YKPRO", "10MAN", "507VH", 3, "", "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public void RegisterDeviceValidation_InputDataDeviceWithDeviceType_ReturnTrue(string deviceName, string crPlace, string crStore,
           int tillNumber, string deviceType, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange      
            RegisterDeviceCommand registerDeviceCommand = new()
            {
                DeviceName = deviceName,
                CrPlace = crPlace,
                CrStore = crStore,
                TillNumber = tillNumber,
                DeviceType = deviceType,
                MacAddress = macAddress,
                IP = ip,
                Processor = processor,
                NetworkCard = networkCard,
                IsServer = isServer,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            //Act
            var getDevicesHandler = new RegisterDeviceHandler(logService.Object, context);
            var response = getDevicesHandler.Handle(registerDeviceCommand, new CancellationToken());


            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsCompleted);
            Assert.True(response.Result);
        }

        [Theory]
        [InlineData("MAN9YKOMAR", "10MAN", "50D49", 1, "55:4A:2B:37:F3:56", "10.106.01.116", "BFEBFBFF00031777", "3217AE75-1B98-1B6F-907K-2CQ29470G31A", true)]
        [InlineData("MAN9YKPRO", "10MAN", "509YK", 1, "43:2A:3C:32:F1:64", "10.200.13.223", "BFEBFAQF00020655", "1237EE16-1B21-4B1F-904G-2CE20431F56E", false)]
        [InlineData("MAN9YKPRO", "10MAN", "507VH", 3, "88:4A:2B:37:F3:55", "10.106.11.236", "BFEBFBFF00031767", "3217EE16-4B32-4B5F-904F-2CE29431F42C", false)]
        public void RegisterUpdateDeviceExist_InputDataDevice_ReturnNotNullIsCompletedTrue(string deviceName, string crPlace, string crStore,
          int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            AddDevicesForUpdate();
            RegisterDeviceCommand request = new RegisterDeviceCommand()
            {
                DeviceName = deviceName,
                CrPlace = crPlace,
                CrStore = crStore,
                TillNumber = tillNumber,
                MacAddress = macAddress,
                IP = ip,
                Processor = processor,
                NetworkCard = networkCard,
                IsServer = isServer,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            //Act
            var registerDevicesHandler = new RegisterDeviceHandler(logService.Object, context);
            var response = registerDevicesHandler.Handle(request, new CancellationToken());

            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsCompleted);
            Assert.True(response.Result);
        }

        [Theory]
        [InlineData("MAN9YKOMAR", "10MAN", "509YK", 1, "55:4A:2B:37:F3:56", "10.106.01.111", "BFEBFBAA43231777", "3217AE65-1B98-1B6F-907K-2CQ29470G31A", false)]
        [InlineData("MAN9YKPRO", "10MAN", "507VH", 1, "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", true)]
        [InlineData("MAN9YKPRO", "10MAN", "50D49", 3, "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public void RegisterUpdateDeviceDeprecated_InputDataDevice_ReturnNotNullIsCompletedTrue(string deviceName, string crPlace, string crStore,
         int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            AddDevicesForUpdateDeprecated();
            RegisterDeviceCommand request = new()
            {
                DeviceName = deviceName,
                CrPlace = crPlace,
                CrStore = crStore,
                TillNumber = tillNumber,
                MacAddress = macAddress,
                IP = ip,
                Processor = processor,
                NetworkCard = networkCard,
                IsServer = isServer,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398"
            };

            //Act
            var registerDevicesHandler = new RegisterDeviceHandler(logService.Object, context);
            var response = registerDevicesHandler.Handle(request, new CancellationToken());

            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsCompleted);
            Assert.True(response.Result);
        }


        [Theory]
        [InlineData("MAN9YKOMAR", "10MAN", "123YK", 1, "55:4A:2B:37:F3:56", "10.106.01.111", "BFEBFBAA43231777", "3217AE65-1B98-1B6F-907K-2CQ29470G31A", false)]
        [InlineData("MAN9YKPRO", "10MAN", "234VH", 1, "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", true)]
        [InlineData("MAN9YKPRO", "10MAN", "12D49", 3, "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public void RegisterDevice_InputDataDevice_ReturnExecption(string deviceName, string crPlace, string crStore,
         int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommand request = new()
            {
                DeviceName = deviceName,
                CrPlace = crPlace,
                CrStore = crStore,
                TillNumber = tillNumber,
                MacAddress = macAddress,
                IP = ip,
                Processor = processor,
                NetworkCard = networkCard,
                IsServer = isServer
            };

            //Act
            var registerDevicesHandler = new RegisterDeviceHandler(logService.Object, context);
            var response = registerDevicesHandler.Handle(request, new CancellationToken());

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Exception);
            Assert.NotNull(response.Exception?.InnerException);
        }

        private static List<StorePlace> AddStorePlaceMockData()
        {
            List<StorePlace> PLACES = new()
            {
                new StorePlace()
                {
                    STORE_PLACE_ID = 1,
                    CR_STORE = "509YK",
                    CR_PLACE = "10MAN",
                    IS_ACTIVE = true
                },
                new StorePlace()
                {
                    STORE_PLACE_ID = 2,
                    CR_STORE = "50D49",
                    CR_PLACE = "10MAN",
                    IS_ACTIVE = true
                },
                new StorePlace()
                {
                    STORE_PLACE_ID = 3,
                    CR_STORE = "507VH",
                    CR_PLACE = "10MAN",
                    IS_ACTIVE = true
                }
            };
            return PLACES;
        }
        private static List<DeviceNumber> AddDeviceNumberMockData()
        {
            List<DeviceNumber> DEVICES_NUMBER = new();
            DEVICES_NUMBER.Add(new DeviceNumber()
            {
                DEVICE_NUMBER_ID = 1,
                NUMBER = 1,
                IS_ACTIVE = true
            });
            DEVICES_NUMBER.Add(new DeviceNumber()
            {
                DEVICE_NUMBER_ID = 2,
                NUMBER = 2,
                IS_ACTIVE = true
            });
            DEVICES_NUMBER.Add(new DeviceNumber()
            {
                DEVICE_NUMBER_ID = 3,
                NUMBER = 3,
                IS_ACTIVE = true
            });
            return DEVICES_NUMBER;
        }
        private static List<DeviceType> AddDeviceTypeMockData()
        {
            List<DeviceType> DEVICES_TYPE = new()
            {
                new DeviceType()
                {
                    DEVICE_TYPE_ID = 1,
                    CODE = "DEVICE",
                    NAME = "DEVICE",
                    DESCRIPTION = "DEVICE",
                    IS_ACTIVE = true
                },
                new DeviceType()
                {
                    DEVICE_TYPE_ID = 2,
                    CODE = "TILL",
                    NAME = "TILL",
                    DESCRIPTION = "DEVICE",
                    IS_ACTIVE = true
                }
            };
            return DEVICES_TYPE;
        }
        private static List<DeviceStatus> AddDeviceStatusMockData()
        {
            List<DeviceStatus> DEVICE_STATUS = new()
            {
                new DeviceStatus()
                {
                    DEVICE_STATUS_ID = 1,
                    CODE = "OPEN",
                    NAME = "OPEN",
                    DESCRIPTION = "OPEN",
                    IS_ACTIVE = true
                },
                new DeviceStatus()
                {
                    DEVICE_STATUS_ID = 2,
                    CODE = "CLOSED",
                    NAME = "CLOSED",
                    DESCRIPTION = "CLOSED DEVICE",
                    IS_ACTIVE = true
                },
                new DeviceStatus()
                {
                    DEVICE_STATUS_ID = 3,
                    CODE = "DEPRECATED",
                    NAME = "DEPRECATED",
                    DESCRIPTION = "DEPRECATED DEVICE",
                    IS_ACTIVE = true
                }
            };
            return DEVICE_STATUS;
        }
        private static List<ValidIPRange> AddValidIPRangeMockData()
        {
            List<ValidIPRange> VALID_IP_RANGE = new()
            {
                new ValidIPRange()
                {
                    VALID_IP_RANGE_IP = 1,
                    IP_RANGE = "10.106",
                    DESCRIPTION = string.Empty,
                    IS_ACTIVE = true
                },
                new ValidIPRange()
                {
                    VALID_IP_RANGE_IP = 2,
                    IP_RANGE = "10.200",
                    DESCRIPTION = string.Empty,
                    IS_ACTIVE = true
                }
            };
            return VALID_IP_RANGE;
        }
        private void AddDevicesForUpdate()
        {
            List<Domain.Entities.Device> DEVICES = new()
            {
                new Domain.Entities.Device()
                {
                    DEVICE_ID = Guid.Empty,
                    NAME = "MAN9YKPRO",
                    MAC_ADDRESS = "99:4A:2B:37:F3:66",
                    IP = "10.106.11.226",
                    STORE_PLACE_ID = 3,
                    DEVICE_NUMBER_ID = 3,
                    DEVICE_TYPE_ID = 2,
                    DEVICE_STATUS_ID = 1,
                    PROCESSOR = "BFEBFBFF00031777",
                    NETWORK_CARD = "3217EE16-2B38-4B5F-904F-2CE29431F21A",
                    IS_SERVER = true,
                    IS_ACTIVE = false,
                    CREATED_DATETIME = DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    MODIFIED_DATETIME = null,
                    MODIFIED_BY_OPERATOR_ID = null,
                    LCOUNT = 1
                },
                new Domain.Entities.Device()
                {
                    DEVICE_ID = Guid.Empty,
                    NAME = "MAN9YKPRO",
                    MAC_ADDRESS = "44:8A:5B:37:F1:64",
                    IP = "10.200.10.223",
                    STORE_PLACE_ID = 1,
                    DEVICE_NUMBER_ID = 1,
                    DEVICE_TYPE_ID = 2,
                    DEVICE_STATUS_ID = 1,
                    PROCESSOR = "BFEBFBFF00020655",
                    NETWORK_CARD = "3217EE16-2B38-4B5F-904F-2CE29431F56E",
                    IS_SERVER = true,
                    IS_ACTIVE = true,
                    CREATED_DATETIME = DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    MODIFIED_DATETIME = null,
                    MODIFIED_BY_OPERATOR_ID = null,
                    LCOUNT = 1
                },
                new Domain.Entities.Device()
                {
                    DEVICE_ID = Guid.Empty,
                    NAME = "MAN9YKOMAR",
                    MAC_ADDRESS = "55:4A:2B:37:F3:56",
                    IP = "10.106.11.126",
                    STORE_PLACE_ID = 2,
                    DEVICE_NUMBER_ID = 1,
                    DEVICE_TYPE_ID = 2,
                    DEVICE_STATUS_ID = 1,
                    PROCESSOR = "BFEBFBFF00031777",
                    NETWORK_CARD = "3217EE16-2B38-4B5F-904F-2CE29431F21A",
                    IS_SERVER = false,
                    IS_ACTIVE = false,
                    CREATED_DATETIME = DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    MODIFIED_DATETIME = null,
                    MODIFIED_BY_OPERATOR_ID = null,
                    LCOUNT = 1
                }
            };
            context.DEVICE.AddRange(DEVICES);
            context.SaveChanges();
        }
        private void AddDevicesForUpdateDeprecated()
        {
            List<Domain.Entities.Device> DEVICES = new()
            {
                new Domain.Entities.Device()
                {
                    DEVICE_ID = Guid.Empty,
                    NAME = "MAN9YKPRO",
                    MAC_ADDRESS = "99:4A:2B:37:F3:66",
                    IP = "10.106.11.226",
                    STORE_PLACE_ID = 3,
                    DEVICE_NUMBER_ID = 3,
                    DEVICE_TYPE_ID = 2,
                    DEVICE_STATUS_ID = 1,
                    PROCESSOR = "BFEBFBFF00031777",
                    NETWORK_CARD = "3217EE16-2B38-4B5F-904F-2CE29431F21A",
                    IS_SERVER = true,
                    IS_ACTIVE = false,
                    CREATED_DATETIME = DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    MODIFIED_DATETIME = null,
                    MODIFIED_BY_OPERATOR_ID = null,
                    LCOUNT = 1
                },
                new Domain.Entities.Device()
                {
                    DEVICE_ID = Guid.Empty,
                    NAME = "MAN9YKPRO",
                    MAC_ADDRESS = "44:8A:5B:37:F1:64",
                    IP = "10.200.10.223",
                    STORE_PLACE_ID = 1,
                    DEVICE_NUMBER_ID = 1,
                    DEVICE_TYPE_ID = 2,
                    DEVICE_STATUS_ID = 1,
                    PROCESSOR = "BFEBFBFF00020655",
                    NETWORK_CARD = "3217EE16-2B38-4B5F-904F-2CE29431F56E",
                    IS_SERVER = true,
                    IS_ACTIVE = true,
                    CREATED_DATETIME = DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    MODIFIED_DATETIME = null,
                    MODIFIED_BY_OPERATOR_ID = null,
                    LCOUNT = 1
                },
                new Domain.Entities.Device()
                {
                    DEVICE_ID = Guid.Empty,
                    NAME = "MAN9YKOMAR",
                    MAC_ADDRESS = "55:4A:2B:37:F3:56",
                    IP = "10.106.01.111",
                    STORE_PLACE_ID = 2,
                    DEVICE_NUMBER_ID = 1,
                    DEVICE_TYPE_ID = 2,
                    DEVICE_STATUS_ID = 1,
                    PROCESSOR = "BFEBFBAA43231777",
                    NETWORK_CARD = "3217AE65-1B98-1B6F-907K-2CQ29470G31A",
                    IS_SERVER = false,
                    IS_ACTIVE = false,
                    CREATED_DATETIME = DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    MODIFIED_DATETIME = null,
                    MODIFIED_BY_OPERATOR_ID = null,
                    LCOUNT = 1
                }
            };
            context.DEVICE.AddRange(DEVICES);
            context.SaveChanges();
        }
        private static Workgroup AddDefaultWorkgroupData()
        {
            return new Workgroup()
            {
                WORKGROUP_ID = 1,
                CODE = "DEVICE_TILL",
                NAME = "DEVICE_TILL",
                DESCRIPTION = "DEVICE_TILL",
                IS_ACTIVE = true,
                CREATED_DATETIME = DateTime.UtcNow,
                CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                MODIFIED_DATETIME = null,
                MODIFIED_BY_OPERATOR_ID = null,
                LCOUNT = 1
            };
        }
    }
}
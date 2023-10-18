using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;
using Oxxo.Cloud.Security.Infrastructure.Services;

namespace Oxxo.Cloud.Security.Application.Device.Commands.Register
{
    public class RegisterDeviceCommandValidatorTest
    {
        private readonly CryptographyService cryptographyService;
        readonly ApplicationDbContext context;
        readonly Mock<IMediator> Mediator;

        private readonly string publicKey = "<RSAKeyValue><ModulusfZeLYGkkCYvDLqtLnqjxAE4i90uG/+YUaRvrdOWdQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private readonly string privateKey = "<RSAKeyValue><Modulus>rAE4i90uG/+YUaRvrdOWdQ==</Modulus><Exponent>AQAB</Exponent><P>xd5dr9/ZDt</D></RSAKeyValue>";

        public RegisterDeviceCommandValidatorTest()
        {
            Mediator = new Mock<IMediator>();
            cryptographyService = new CryptographyService(publicKey, privateKey);
            List<StorePlace> PLACES = AddStorePlaceMockData();
            List<DeviceNumber> DEVICES_NUMBER = AddDeviceNumberMockData();
            List<DeviceType> DEVICES_TYPE = AddDeviceTypeMockData();
            List<DeviceStatus> DEVICE_STATUS = AddDeviceStatusMockData();
            List<ValidIPRange> VALID_IP_RANGE = AddValidIPRangeMockData();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            context = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.STORE_PLACE.AddRange(PLACES);
            context.DEVICE_NUMBER.AddRange(DEVICES_NUMBER);
            context.DEVICE_TYPE.AddRange(DEVICES_TYPE);
            context.DEVICE_STATUS.AddRange(DEVICE_STATUS);
            context.VALID_IP_RANGE.AddRange(VALID_IP_RANGE);
            context.SaveChanges();
        }


        [Theory]
        [InlineData("8b6043a96b312f6a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "MAN9YKPRO", "10MAN", "509YK", 1, "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de24b045bb72f50cf2f55b5bab9c53a6b352fbd533470426cf38a", "MAN9YKOMAR", "10MAN", "50D49", 1, "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "10MAN", "507VH", 3, "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public async Task RegisterDeviceValidation_InputDataDevice_ReturnTrue(string deviceIdentifier, string deviceName, string crPlace, string crStore,
            int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var cmd = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
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
            var commandResponse = await commandValidator.ValidateAsync(cmd);
            //Assert
            Assert.True(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("8b6043a96b312f6a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "MAN9YKPRO", "10MAN", "509YK", 1, "TILL", "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de24b045bb72f50cf2f55b5bab9c53a6b352fbd533470426cf38a", "MAN9YKOMAR", "10MAN", "50D49", 1, "DEVICE", "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "10MAN", "507VH", 3, "", "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public async Task RegisterDeviceValidation_InputDataDeviceWithDeviceType_ReturnTrue(string deviceIdentifier, string deviceName, string crPlace, string crStore,
           int tillNumber, string deviceType, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var cmd = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
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
            var commandResponse = await commandValidator.ValidateAsync(cmd);
            //Assert
            Assert.True(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("8b6043a96b312f6a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "", "10MAN", "509YK", 1, "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de24b045bb72f50cf2f55b5bab9c53a6b352fbd533470426cf38a", "", "10MAN", "50D49", 1, "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        public async Task RegisterDeviceValidation_InputDataDeviceName_ReturnFalse(string deviceIdentifier, string deviceName, string crPlace, string crStore,
            int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var cmd = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
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
            var commandResponse = await commandValidator.ValidateAsync(cmd);
            //Assert
            Assert.False(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("8b6043a96b312f6a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "MAN9YKPRO", "", "509YK", 1, "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de24b045bb72f50cf2f55b5bab9c53a6b352fbd533470426cf38a", "MAN9YKOMAR", "11MAN", "50D49", 1, "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "MAN", "507VH", 3, "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public async Task RegisterDeviceValidation_InputDataCrPlace_ReturnFalse(string deviceIdentifier, string deviceName, string crPlace, string crStore,
        int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var cmd = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
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
            var commandResponse = await commandValidator.ValidateAsync(cmd);
            //Assert
            Assert.False(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("8b6043a96b312f6a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "MAN9YKPRO", "10MAN", "", 1, "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de24b045bb72f50cf2f55b5bab9c53a6b352fbd533470426cf38a", "MAN9YKOMAR", "10MAN", "51D59", 1, "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "10MAN", "777VH", 3, "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public async Task RegisterDeviceValidation_InputDataCrStore_ReturnFalse(string deviceIdentifier, string deviceName, string crPlace, string crStore,
            int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var cmdStore = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
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
            var commandResponse = await commandValidator.ValidateAsync(cmdStore);
            //Assert
            Assert.False(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("8b6043a96b312f6a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "MAN9YKPRO", "10MAN", "509YK", -1, "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de24b045bb72f50cf2f55b5bab9c53a6b352fbd533470426cf38a", "MAN9YKOMAR", "10MAN", "50D49", 0, "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "10MAN", "507VH", 10, "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public async Task RegisterDeviceValidation_InputDataTillNumber_ReturnFalse(string deviceIdentifier, string deviceName, string crPlace, string crStore,
            int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var cmdTillNumber = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
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
            var commandResponse = await commandValidator.ValidateAsync(cmdTillNumber);
            //Assert
            Assert.False(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("8b6043a96b312f6a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "MAN9YKPRO", "10MAN", "509YK", 1, "Tillll", "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de24b045bb72f50cf2f55b5bab9c53a6b352fbd533470426cf38a", "MAN9YKOMAR", "10MAN", "50D49", 1, "DEVICEEEEE", "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "10MAN", "507VH", 3, "BOX", "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public async Task RegisterDeviceValidation_InputDataDeviceType_ReturnFalse(string deviceIdentifier, string deviceName, string crPlace, string crStore,
            int tillNumber, string deviceType, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var cmd = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
                DeviceName = deviceName,
                CrPlace = crPlace,
                CrStore = crStore,
                TillNumber = tillNumber,
                DeviceType = deviceType,
                MacAddress = macAddress,
                IP = ip,
                Processor = processor,
                NetworkCard = networkCard,
                IsServer = isServer
            };
            //Act
            var commandResponse = await commandValidator.ValidateAsync(cmd);
            //Assert
            Assert.False(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("8b6043a96b312f6a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "MAN9YKPRO", "10MAN", "509YK", 1, "44:8A:5B:37:F1:64", "312.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de24b045bb72f50cf2f55b5bab9c53a6b352fbd533470426cf38a", "MAN9YKOMAR", "10MAN", "50D49", 1, "55:4A:2B:37:F3:56", "222.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "10MAN", "507VH", 3, "99:4A:2B:37:F3:66", "", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public async Task RegisterDeviceValidation_InputDataIP_ReturnFalse(string deviceIdentifier, string deviceName, string crPlace, string crStore,
            int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var cmdIp = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
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
            var commandResponse = await commandValidator.ValidateAsync(cmdIp);
            //Assert
            Assert.False(commandResponse.IsValid);
        }


        [Theory]
        [InlineData("8b6043a96b312f6a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "MAN9YKPRO", "10MAN", "509YK", 1, "", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de24b045bb72f50cf2f55b5bab9c53a6b352fbd533470426cf38a", "MAN9YKOMAR", "10MAN", "50D49", 1, "55:4A:2B:37:F3:56", "10.106.11.226", "", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("7a4fb8431083b315e5ccdac70dac8d980b0fbf88b7dbe7f70212843a1f4bc5c1", "MAN9YKPRO", "10MAN", "507VH", 3, "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "", false)]
        public async Task RegisterDeviceValidation_InputDataDeviceMacAddressProcessorNetWorkCard_ReturnFalse(string deviceIdentifier, string deviceName, string crPlace, string crStore,
            int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var registerDevice = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
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
            var commandResponse = await commandValidator.ValidateAsync(registerDevice);
            //Assert
            Assert.False(commandResponse.IsValid);
        }

        [Theory]
        [InlineData("3asd12331a22fce42cb3be493ddadaf93026b863254adc4d9e7a134d51", "MAN9YKPRO", "10MAN", "509YK", 1, "44:8A:5B:37:F1:64", "10.200.10.223", "BFEBFBFF00020655", "3217EE16-2B38-4B5F-904F-2CE29431F56E", false)]
        [InlineData("d1b4de9baa6de2fh23yajsfqa1250cf2f55b5bab9c53a6b352fbd533470426cf38a", "MAN9YKOMAR", "10MAN", "50D49", 1, "55:4A:2B:37:F3:56", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", true)]
        [InlineData("", "MAN9YKPRO", "10MAN", "507VH", 3, "99:4A:2B:37:F3:66", "10.106.11.226", "BFEBFBFF00031777", "3217EE16-2B38-4B5F-904F-2CE29431F21A", false)]
        public async Task RegisterDeviceValidation_InputDataDeviceIdentifier_ReturnFalse(string deviceIdentifier, string deviceName, string crPlace, string crStore,
            int tillNumber, string macAddress, string ip, string processor, string networkCard, bool isServer)
        {
            //Arrange
            RegisterDeviceCommandValidator commandValidator = new(context, cryptographyService);
            var cmdIdentifier = new RegisterDeviceCommand()
            {
                DeviceIdentifier = deviceIdentifier,
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
            var commandResponse = await commandValidator.ValidateAsync(cmdIdentifier);
            //Assert
            Assert.False(commandResponse.IsValid);
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
            List<DeviceNumber> DEVICES_NUMBER = new()
            {
                new DeviceNumber()
                {
                    DEVICE_NUMBER_ID = 1,
                    NUMBER = 1,
                    IS_ACTIVE = true
                },
                new DeviceNumber()
                {
                    DEVICE_NUMBER_ID = 2,
                    NUMBER = 2,
                    IS_ACTIVE = true
                },
                new DeviceNumber()
                {
                    DEVICE_NUMBER_ID = 3,
                    NUMBER = 3,
                    IS_ACTIVE = true
                }
            };
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
    }
}

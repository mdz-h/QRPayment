using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;


namespace Oxxo.Cloud.Security.Application.Device.Commands.Enabled
{
    public class EnabledDeviceCommandTest
    {
        readonly ApplicationDbContext context;
        private Mock<ILogService> logService;
        readonly Mock<IMediator> Mediator;

        public EnabledDeviceCommandTest()
        {

            this.logService = new Mock<ILogService>();
            Mediator = new Mock<IMediator>();

            List<Domain.Entities.Device> DEVICES = AddDeviceMockData();
            List<DeviceStatus> DEVICES_STATUS = AddDeviceStatusMockData();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .Options;
            context = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor());
            context.DEVICE.AddRange(DEVICES);
            context.DEVICE_STATUS.AddRange(DEVICES_STATUS);

            context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <param name="Enabled"></param>

        [Theory]
        [InlineData("eed4684a-e7bb-49ec-a474-e25b732ab184", true)]
        [InlineData("eed4684a-e7bb-49ec-a474-e25b732ab184", false)]
        public void EnabledDeviceCommand_ValidateHandle_ReturnBool(string DeviceId, bool Enabled)
        {
            EnabledDeviceCommand command = new()
            {
                DeviceId = DeviceId,
                Enabled = Enabled
            };
            var handler = new EnabledDeviceCommandHandler(context, logService.Object);
            var result = handler.Handle(command, new CancellationToken());
            Assert.NotNull(result);
        }


        private List<Domain.Entities.Device> AddDeviceMockData()
        {

            List<Domain.Entities.Device> DEVICES = new List<Domain.Entities.Device>();
            DEVICES.Add(new Domain.Entities.Device()
            {
                DEVICE_ID = new Guid("eed4684a-e7bb-49ec-a474-e25b732ab184"),
                DEVICE_STATUS_ID = 1,
                IS_ACTIVE = true,

            });
            DEVICES.Add(new Domain.Entities.Device()
            {
                DEVICE_ID = new Guid("31F79AEA-DAD5-4CC1-831D-754BEC088654"),
                DEVICE_STATUS_ID = 2,
                IS_ACTIVE = false,

            });
            DEVICES.Add(new Domain.Entities.Device()
            {
                DEVICE_ID = new Guid("451E6FAF-F9AA-4182-B870-313ED08CAF97"),
                DEVICE_STATUS_ID = 3,
                IS_ACTIVE = false,

            });

            return DEVICES;
        }

        private List<DeviceStatus> AddDeviceStatusMockData()
        {
            List<DeviceStatus> DEVICE_STATUS = new List<DeviceStatus>();
            DEVICE_STATUS.Add(new DeviceStatus()
            {
                DEVICE_STATUS_ID = 1,
                CODE = "OPEN",
                NAME = "OPEN",
                DESCRIPTION = "OPEN",
                IS_ACTIVE = true
            });
            DEVICE_STATUS.Add(new DeviceStatus()
            {
                DEVICE_STATUS_ID = 2,
                CODE = "CLOSED",
                NAME = "CLOSED",
                DESCRIPTION = "CLOSED DEVICE",
                IS_ACTIVE = true
            });
            DEVICE_STATUS.Add(new DeviceStatus()
            {
                DEVICE_STATUS_ID = 3,
                CODE = "DEPRECATED",
                NAME = "DEPRECATED",
                DESCRIPTION = "DEPRECATED DEVICE",
                IS_ACTIVE = true
            });
            return DEVICE_STATUS;
        }

    }
}

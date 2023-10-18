using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Device.Commands.Enabled
{
    public class EnabledDeviceCommandValidatiorTest : AbstractValidator<EnabledDeviceCommand>
    {
        public EnabledDeviceCommandValidatiorTest()
        {

            RuleFor(v => v.DeviceId)
                .NotEmpty().WithMessage(GlobalConstantMessages.ENABLEDDEVICEIDNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.Enabled)
                .NotNull().WithMessage(GlobalConstantMessages.ENABLEDDEVICEONOFFEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }


        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="DeviceId">Short name permission</param>
        /// <param name="Enabled">Code Permission</param>
        [Theory]
        [InlineData("", true)]
        [InlineData("693AAAEE-256C-ED11-ADE5-A04A5E6D758C", null)]
        [InlineData("", null)]
        public void EnabledDeviceCommand_ValidationNullFields(string DeviceId, bool? Enabled)
        {
            EnabledDeviceCommand command = new()
            {
                DeviceId = DeviceId,
                Enabled = Enabled
            };

            var validator = new EnabledDeviceCommandValidatiorTest();
            var result = validator.Validate(command);

            Assert.NotNull(result);


        }
    }
}

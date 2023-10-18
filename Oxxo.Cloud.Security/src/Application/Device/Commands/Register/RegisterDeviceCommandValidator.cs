#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-11-23.
// Comment: Validator device register.
//===============================================================================
#endregion

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Device.Commands.Register
{
    public class RegisterDeviceCommandValidator : AbstractValidator<RegisterDeviceCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ICryptographyService cryptographyService;
        private bool validate = false;

        /// <summary>
        /// Constructor that injects the database context and cryptography. 
        /// Contains the rules for validation data.
        /// </summary>
        /// <param name="authenticateQuery">Inject the interface with the necessary methods to obtener consults in the database</param>
        /// <param name="cryptographyService">Inject the interface with the necessary methods to encrypt and decrypt</param>
        public RegisterDeviceCommandValidator(IApplicationDbContext context, ICryptographyService cryptographyService)
        {
            this.context = context;
            this.cryptographyService = cryptographyService;
            RuleFor(v => v.DeviceName).NotEmpty().WithMessage(GlobalConstantMessages.DEVICENAMENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.CrPlace).NotEmpty().WithMessage(GlobalConstantMessages.CRPLACENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
           .MustAsync(ValidateCrPlace).WithMessage(GlobalConstantMessages.CRPlACENOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

            RuleFor(v => v.CrStore).NotEmpty().WithMessage(GlobalConstantMessages.CRSTORENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
            .MustAsync(ValidateCrStore).WithMessage(GlobalConstantMessages.CRSTORENOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

            RuleFor(v => v.TillNumber).NotEqual(0).WithMessage(GlobalConstantMessages.TILLNUMBERNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
            .MustAsync(ValidateTillNumber).WithMessage(GlobalConstantMessages.TILLNUMBERNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

            RuleFor(v => v.DeviceType).MustAsync(ValidateDeviceType).WithMessage(GlobalConstantMessages.DEVICETYPENOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

            RuleFor(v => v.IP).NotEmpty().WithMessage(GlobalConstantMessages.IPNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateIP).WithMessage(GlobalConstantMessages.IPNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

            RuleFor(v => v.MacAddress).NotEmpty().WithMessage(GlobalConstantMessages.MACADDRESSNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(v => v.Processor).NotEmpty().WithMessage(GlobalConstantMessages.PROCESSORNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(v => v.NetworkCard).NotEmpty().WithMessage(GlobalConstantMessages.NETWORKCARDNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(v => v).Must(ValidateIdentifier).WithMessage(GlobalConstantMessages.DEVICEIDENTIFIERNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
            RuleFor(v => v.Identification).NotEmpty().WithMessage(GlobalConstantMessages.IDENTIFICATIONOPERATORDEFUALT).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }
        
        private async Task<bool> ValidateCrPlace(string? crPlace, CancellationToken cancellationToken)
        {
            return this.validate = await context.STORE_PLACE.AsNoTracking().AnyAsync(x => x.CR_PLACE == crPlace && x.IS_ACTIVE, cancellationToken: cancellationToken);
        }

        private async Task<bool> ValidateCrStore(string? crStore, CancellationToken cancellationToken)
        {
            if (!validate)
            {
                return true;
            }
            return this.validate = await context.STORE_PLACE.AsNoTracking().AnyAsync(x => x.CR_STORE == crStore && x.IS_ACTIVE, cancellationToken: cancellationToken);
        }

        private async Task<bool> ValidateTillNumber(int tillNumber, CancellationToken cancellationToken)
        {
            if (!validate)
            {
                return true;
            }
            return this.validate = await context.DEVICE_NUMBER.AsNoTracking().AnyAsync(x => x.NUMBER == tillNumber && x.IS_ACTIVE, cancellationToken: cancellationToken);
        }

        private async Task<bool> ValidateDeviceType(string? deviceType, CancellationToken cancellationToken)
        {
            if (!validate || string.IsNullOrEmpty(deviceType))
            {
                return true;
            }

            return this.validate = await context.DEVICE_TYPE.AsNoTracking().AnyAsync(x => x.CODE == deviceType && x.IS_ACTIVE, cancellationToken: cancellationToken);
        }

        private async Task<bool> ValidateIP(string? ip, CancellationToken cancellationToken)
        {
            if (!validate)
            {
                return true;
            }

            if (!IPAddress.TryParse(ip, out IPAddress? addr))
            {
                return this.validate = false;
            }

            string ipRange = $"{ip.Split(".").GetValue(0)?.ToString()}.{ip.Split(".").GetValue(1)?.ToString()}";
            return this.validate = await context.VALID_IP_RANGE.AsNoTracking().AnyAsync(x => x.IP_RANGE == ipRange && x.IS_ACTIVE, cancellationToken: cancellationToken);

        }

        private bool ValidateIdentifier(RegisterDeviceCommand registerDeviceCommand)
        {
            if (!validate)
            {
                return true;
            }
            var deviceKey = string.Concat("{", registerDeviceCommand.CrPlace, "}-{", registerDeviceCommand.CrStore, "}-{", registerDeviceCommand.TillNumber, "}-{", registerDeviceCommand.MacAddress, "}-{", registerDeviceCommand.IP, "}-{", registerDeviceCommand.Processor, "}-{", registerDeviceCommand.NetworkCard, "}");
            if (cryptographyService.EncryptHash(deviceKey) == registerDeviceCommand.DeviceIdentifier)
            {
                return validate = true;
            }
            else
            {
                return validate = false;
            }
        }
        
    }
}

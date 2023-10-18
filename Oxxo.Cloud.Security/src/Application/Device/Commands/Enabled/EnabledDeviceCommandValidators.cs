#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-11-23.
// Comment: Command device enabled validator.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Device.Commands.Enabled
{
    /// <summary>
    /// Principal class
    /// </summary>
    public class EnabledDeviceCommandValidators : AbstractValidator<EnabledDeviceCommand>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor EnabledDeviceCommandValidators that injects the logService, authenticateQuery, tokenGenerator and database context
        /// <param name="context"></param>
        /// </summary>
        public EnabledDeviceCommandValidators(IApplicationDbContext context)
        {
            this.context = context;             
            RuleFor(v => v.Enabled).NotNull().WithMessage(GlobalConstantMessages.ENABLEDDEVICEONOFFEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(v => v.DeviceId).NotEmpty().WithMessage(GlobalConstantMessages.ENABLEDDEVICEIDNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidatedeviceId).WithMessage(GlobalConstantMessages.ENABLEDDEVICEIDNOTEMPTY).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());           
        }

        /// <summary>
        /// Validate if field device_id exist the registrer in databases
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ValidatedeviceId(string deviceId, CancellationToken cancellationToken)
        {
            return await this.context.DEVICE.AsNoTracking()
                .Include(i => i.DEVICE_STATUS)
                .AnyWithNoLockAsync(w => w.DEVICE_ID.ToString() == deviceId && w.DEVICE_STATUS.CODE != GlobalConstantHelpers.DEVICESTATUSDEPRECATED, cancellationToken);
        }
    }
}

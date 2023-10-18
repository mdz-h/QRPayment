#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-11-23.
// Comment: Command device enabled.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Device.Commands.Enabled
{
    /// <summary>
    /// DTO class
    /// </summary>
    public class EnabledDeviceCommand : BasePropertiesDto, IRequest<bool>
    {
        public EnabledDeviceCommand()
        {
            DeviceId = string.Empty;
        }

        public string DeviceId { get; set; }
        public bool? Enabled { get; set; }
    }

    /// <summary>
    /// Principal Class
    /// </summary>
    public class EnabledDeviceCommandHandler : IRequestHandler<EnabledDeviceCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Inicialized constructor by EnabledDeviceCommandHandler inject logService and context database
        /// </summary>
        /// <param name="context">Context database</param>
        /// <param name="logService">Log Instance</param>
        public EnabledDeviceCommandHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }

        /// <summary>
        /// main method
        /// </summary>
        /// <param name="request">Request app</param>
        /// <param name="cancellationToken">Cancellationtoken</param>
        /// <returns></returns>
        /// <exception cref="CustomException">Excepction</exception>
        public async Task<bool> Handle(EnabledDeviceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await context.DEVICE.Where(x => x.DEVICE_ID.ToString() == request.DeviceId).FirstOrDefaultAsync(cancellationToken);
                if (result == null || request.Enabled == null)
                {
                    throw new CustomException(GlobalConstantMessages.ENABLEDDEVICENOTFOUND, HttpStatusCode.UnprocessableEntity);
                }

                result.IS_ACTIVE = request.Enabled.Value;
                result.LCOUNT += 1;
                result.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                result.MODIFIED_DATETIME = DateTime.UtcNow;
                await this.context.SaveChangesAsync(cancellationToken);
               
                return true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODENABLEDDEVICE, GlobalConstantHelpers.METHODENABLED, LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.ENABLEDDEVICENOTFOUND, ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex.StackTrace), GlobalConstantHelpers.NAMEENABLEDDEVICECONTROLLER);
                throw;
            }
        }
    }
}

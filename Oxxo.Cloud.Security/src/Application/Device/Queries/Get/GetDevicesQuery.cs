#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-11-26.
// Comment: Class that get devices query.
//===============================================================================
#endregion
using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Device.Queries
{
    public class GetDevicesQuery : BasePropertiesDto, IRequest<List<DeviceResponse>>
    {
        public int ItemsNumber { get; set; }
        public int PageNumber { get; set; }
        public string? DeviceIdentifier { get; set; }

    }

    public class GetDevicesHandler : IRequestHandler<GetDevicesQuery, List<DeviceResponse>>
    {
        private readonly ILogService logService;
        private readonly IDeviceQueryGet deviceQuery;
        /// <summary>
        /// Constructor that injects the interface ILogService and IDeviceQuery.
        /// </summary>            
        /// <param name="logService">Inject the interface necessary for save the log</param>
        /// <param name="deviceQuery">Inject the interface necessary for get devices</param>
        public GetDevicesHandler(ILogService logService, IDeviceQueryGet deviceQuery)
        {
            this.logService = logService;
            this.deviceQuery = deviceQuery;
        }

        /// <summary>
        /// Generates the DeviceResponse 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List DeviceResponse</returns>
        public async Task<List<DeviceResponse>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await deviceQuery.GetDevices(request);
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETDEVICES, GlobalConstantHelpers.METHODGETDEVICESHANDLER, LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORGETDEVICESCOMMAND, ex.Message, ex.StackTrace), GlobalConstantHelpers.NAMECLASSGETDEVICESQUERY);

                throw new CustomException(ex.InnerException == null ? ex.Message : ex.InnerException.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}

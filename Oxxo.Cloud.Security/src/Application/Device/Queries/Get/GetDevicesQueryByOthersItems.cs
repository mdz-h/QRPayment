using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Oxxo.Cloud.Security.Application.Device.Queries
{
    public class GetDevicesQueryByOthersItems : BasePropertiesDto, IRequest<List<DeviceResponseByOthersItems>>
    {       
        public int ItemsNumber { get; set; }
        public int PageNumber { get; set; }
        public string CrPlaza { get; set; }
        public string CrTienda { get; set; }
        public string? CajaId { get; set; }
        public string? DeviceIdentifier { get; set; }
    }

    public class GetDevicesByOthersItemsHandler : IRequestHandler<GetDevicesQueryByOthersItems, List<DeviceResponseByOthersItems>>
    {
        private readonly ILogService logService;
        private readonly IDeviceQueryGetByOthersItems deviceQuery;

        /// <summary>
        /// Constructor that injects the interface ILogService and IDeviceQuery.
        /// </summary>            
        /// <param name="logService">Inject the interface necessary for save the log</param>
        /// <param name="deviceQuery">Inject the interface necessary for get devices</param>
        public GetDevicesByOthersItemsHandler(ILogService logService, IDeviceQueryGetByOthersItems deviceQuery)
        {
            this.logService = logService;
            this.deviceQuery = deviceQuery;
        }

        public async Task<List<DeviceResponseByOthersItems>> Handle(GetDevicesQueryByOthersItems request, CancellationToken cancellationToken)
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

#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-11-0630.
// Comment: Command register device.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;

namespace Oxxo.Cloud.Security.Application.Device.Commands.Register
{
    public class RegisterDeviceCommand : BasePropertiesDto, IRequest<bool>
    {
        public string? DeviceIdentifier { get; set; }
        public string DeviceName { get; set; } = string.Empty; 
        public string? CrPlace { get; set; }
        public string? CrStore { get; set; }
        public int TillNumber { get; set; }
        public string MacAddress { get; set; } = string.Empty;
        public string IP { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public string NetworkCard { get; set; } = string.Empty;
        public bool IsServer { get; set; }
        public string? DeviceType { get; set; }
    }

    public class RegisterDeviceHandler : IRequestHandler<RegisterDeviceCommand, bool>
    {
        private readonly ILogService logService;
        private readonly IApplicationDbContext context;
       
        /// <summary>
        /// Constructor that injects the logService and database context.
        /// </summary>
        /// <param name="logService"></param>
        /// <param name="context"></param>
        public RegisterDeviceHandler(ILogService logService, IApplicationDbContext context)
        {
            this.logService = logService;
            this.context = context;           
        }

        /// <summary>
        /// Gets the data required to register or update the device. Calls the method that updates/deprecates the device.
        /// Finally, register the new device
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Boolean with value correct or incorrect</returns>
        public async Task<bool> Handle(RegisterDeviceCommand request, CancellationToken cancellationToken)
        {           
            try
            {
                string nameDeviceType = string.IsNullOrEmpty(request.DeviceType) ? GlobalConstantHelpers.DEVICETYPETILL : request.DeviceType;
                var storePlace = await context.STORE_PLACE.AsNoTracking().Where(x => x.CR_STORE == request.CrStore && x.CR_PLACE == request.CrPlace && x.IS_ACTIVE).FirstAsync(cancellationToken: cancellationToken);
                var deviceType = await context.DEVICE_TYPE.AsNoTracking().Where(x => x.CODE == nameDeviceType && x.IS_ACTIVE).FirstAsync(cancellationToken: cancellationToken);
                var deviceNumber = await context.DEVICE_NUMBER.AsNoTracking().Where(x => x.NUMBER == request.TillNumber && x.IS_ACTIVE).FirstAsync(cancellationToken: cancellationToken);
                var deviceStatus = await context.DEVICE_STATUS.AsNoTracking().Where(x => x.CODE == GlobalConstantHelpers.DEVICESTATUSOPEN && x.IS_ACTIVE).FirstAsync(cancellationToken: cancellationToken);

                var validateUpdate = await GetDeviceUpdate(request, storePlace.STORE_PLACE_ID, deviceType.DEVICE_TYPE_ID, deviceNumber.DEVICE_NUMBER_ID, cancellationToken);
               
                if (validateUpdate)
                {
                    return true;
                }

                await GetDeviceDeprecated(request, cancellationToken);
                int defaultWorkgroupId = await GetDefaultRole(cancellationToken);
                using var transaction = context.database.BeginTransaction();
                try
                {                    
                    var deviceNew = new Domain.Entities.Device()
                    {
                        STORE_PLACE_ID = storePlace.STORE_PLACE_ID,
                        DEVICE_TYPE_ID = deviceType.DEVICE_TYPE_ID,
                        DEVICE_STATUS_ID = deviceStatus.DEVICE_STATUS_ID,
                        DEVICE_NUMBER_ID = deviceNumber.DEVICE_NUMBER_ID,
                        MAC_ADDRESS = request.MacAddress,
                        IP = request.IP,
                        PROCESSOR = request.Processor,
                        NETWORK_CARD = request.NetworkCard,
                        NAME = request.DeviceName,
                        DESCRIPTION = request.DeviceName,
                        IS_SERVER = request.IsServer,
                        IS_ACTIVE = false,
                        LCOUNT = 0,
                        CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                        CREATED_DATETIME = DateTime.UtcNow
                    };
                    await context.DEVICE.AddAsync(deviceNew, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken); 

                    var userWorkgroupLink = new Domain.Entities.UserWorkgroupLink()
                    {
                        GUID = deviceNew.DEVICE_ID,
                        WORKGROUP_ID = defaultWorkgroupId,
                        IS_ACTIVE = true,
                        LCOUNT = 0,
                        CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                        CREATED_DATETIME = DateTime.UtcNow
                    };
                    await context.USER_WORKGROUP_LINK.AddAsync(userWorkgroupLink, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    await transaction.DisposeAsync();
                    throw;
                }                
            }
            catch (Exception ex)
            {                
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODREGISTERDEVICE, GlobalConstantHelpers.METHOREGISTERDEVICEHANDLER, LogTypeEnum.Error, string.Concat(GlobalConstantMessages.LOGERROREGISTERDEVICECOMMAND, ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex.StackTrace, request.Source), GlobalConstantHelpers.NAMECLASSREGISTERDEVICECOMMAND);
                throw;
            }
        }

        /// <summary>
        /// Gets and check if the device exists with all data sent. If it exists, update the information.
        /// </summary>
        /// <param name="request">Objetc RegisterDeviceCommand</param>
        /// <param name="storePlaceId">id of table StorePlace</param>
        /// <param name="deviceTypeId">id of table deviceType</param>
        /// <param name="deviceNumberId">id of table deviceNumber</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Boolean with value correct or incorrect</returns>
        private async Task<bool> GetDeviceUpdate(RegisterDeviceCommand request, int storePlaceId, int deviceTypeId, int deviceNumberId, CancellationToken cancellationToken)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODREGISTERDEVICE, GlobalConstantHelpers.METHOREGISTERDEVICEHANDLER, LogTypeEnum.Information, string.Concat(GlobalConstantMessages.LOGINITUPDATEDEVICECOMMAND, request.Source), GlobalConstantHelpers.NAMECLASSREGISTERDEVICECOMMAND);
            var device = await context.DEVICE
                                 .Include(x => x.STORE_PLACE)
                                 .Include(x => x.DEVICE_NUMBER)
                                 .Where(x => x.STORE_PLACE.CR_PLACE == request.CrPlace && x.STORE_PLACE.CR_STORE == request.CrStore
                                  && x.DEVICE_NUMBER.NUMBER == request.TillNumber && x.NAME == request.DeviceName
                                 && x.DEVICE_STATUS.CODE != GlobalConstantHelpers.DEVICESTATUSDEPRECATED).FirstOrDefaultWithNoLockAsync(cancellationToken: cancellationToken);

            if (device != null)
            {
                device.STORE_PLACE_ID = storePlaceId;
                device.DEVICE_TYPE_ID = deviceTypeId;
                device.DEVICE_NUMBER_ID = deviceNumberId;
                device.MAC_ADDRESS = request.MacAddress;
                device.IP = request.IP;
                device.PROCESSOR = request.Processor;
                device.NETWORK_CARD = request.NetworkCard;
                device.NAME = request.DeviceName;
                device.DESCRIPTION = request.DeviceName;
                device.IS_SERVER = request.IsServer;
                device.LCOUNT += 1;
                device.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                device.MODIFIED_DATETIME = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODREGISTERDEVICE, GlobalConstantHelpers.METHOREGISTERDEVICEHANDLER, LogTypeEnum.Information, string.Concat(GlobalConstantMessages.LOGENDUPDATEDEVICECOMMAND, request.Source), GlobalConstantHelpers.NAMECLASSREGISTERDEVICECOMMAND);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get and check if the device exists with all the data sent but has a different store and place. If it exists, update by assigning the value "DEPRECATED".
        /// </summary>
        /// <param name="request">Objetc RegisterDeviceCommand</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Boolean with value correct or incorrect</returns>
        private async Task<bool> GetDeviceDeprecated(RegisterDeviceCommand request, CancellationToken cancellationToken)
        {
            await logService.Logger(GlobalConstantHelpers.EVENTMETHODREGISTERDEVICE, GlobalConstantHelpers.METHOREGISTERDEVICEHANDLER, LogTypeEnum.Information,
                string.Concat(GlobalConstantMessages.LOGINITDEPRECATEDDEVICECOMMAND, request.Source), GlobalConstantHelpers.NAMECLASSREGISTERDEVICECOMMAND);
            var device = await context.DEVICE
                                .Include(x => x.STORE_PLACE)
                                .Include(x => x.DEVICE_NUMBER)
                                .Where(x => x.MAC_ADDRESS == request.MacAddress && x.PROCESSOR == request.Processor && x.NETWORK_CARD == request.NetworkCard
                                && x.DEVICE_NUMBER.NUMBER == request.TillNumber && (x.STORE_PLACE.CR_STORE != request.CrStore
                                || x.STORE_PLACE.CR_PLACE != request.CrPlace)
                                && x.DEVICE_STATUS.CODE != GlobalConstantHelpers.DEVICESTATUSDEPRECATED).FirstOrDefaultWithNoLockAsync(cancellationToken: cancellationToken);

            if (device != null)
            {
                var deviceStatus = await context.DEVICE_STATUS.AsNoTracking().Where(x => x.CODE == GlobalConstantHelpers.DEVICESTATUSDEPRECATED && x.IS_ACTIVE).FirstAsync(cancellationToken: cancellationToken);

                device.IS_ACTIVE = false;
                device.DEVICE_STATUS_ID = deviceStatus.DEVICE_STATUS_ID;
                device.LCOUNT += 1;
                device.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                device.MODIFIED_DATETIME = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODREGISTERDEVICE, GlobalConstantHelpers.METHOREGISTERDEVICEHANDLER, LogTypeEnum.Information,
                    string.Concat(GlobalConstantMessages.LOGENDDEPRECATEDDEVICECOMMAND, request.Source), GlobalConstantHelpers.NAMECLASSREGISTERDEVICECOMMAND);
                return true;
            }
            return false;
        }

        private async Task<int> GetDefaultRole(CancellationToken cancellationToken)
        {
            var defaultWorkgroup =  await context.WORKGROUP.AsNoTracking().FirstOrDefaultWithNoLockAsync(x=>x.CODE == GlobalConstantHelpers.DEFAULTDEVICEWORKGROUPCODE && x.IS_ACTIVE, cancellationToken);
            if(defaultWorkgroup == null)
            {
                throw new CustomException(GlobalConstantMessages.DEVICEDEFAULTWORKGROUPNOTFOUND, System.Net.HttpStatusCode.UnprocessableEntity);
            }
            return defaultWorkgroup.WORKGROUP_ID;
        }
    }
}

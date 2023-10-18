#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-11-26.
// Comment: Class for Get devices.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;

namespace Oxxo.Cloud.Security.Application.Device.Queries.Get
{
    public class DeviceQueryGet : IDeviceQueryGet
    {
        private readonly IApplicationDbContext Context;

        /// <summary>
        /// Constructor that injects the database context.  
        /// </summary>
        /// <param name="context">Context database</param>        
        public DeviceQueryGet(IApplicationDbContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// This function is responsible that execute query for get devices
        /// </summary>
        /// <param name="request"></param>        
        /// <returns>List DeviceResponse</returns>
        public async Task<List<DeviceResponse>> GetDevices(GetDevicesQuery request)
        {

            return (await Context.DEVICE.AsNoTracking()
                    .Include(device => device.DEVICE_NUMBER).AsNoTracking()
                    .Include(device => device.STORE_PLACE).AsNoTracking()
                    .Include(device => device.DEVICE_STATUS).AsNoTracking()
                    .Include(device => device.DEVICE_TYPE).AsNoTracking()
                    .Where(device => device.STORE_PLACE.IS_ACTIVE
                        && device.DEVICE_NUMBER.IS_ACTIVE
                        && (!string.IsNullOrEmpty(request.DeviceIdentifier) ? device.DEVICE_ID.ToString() == request.DeviceIdentifier : true))
                    .Skip((request.PageNumber - 1) * request.ItemsNumber).Take(request.ItemsNumber).ToListAsync()
                    ).Select(device =>
                        new DeviceResponse()
                        {
                            Device_id = device.DEVICE_ID,
                            Name = device.NAME,
                            Cr_place = device.STORE_PLACE.CR_PLACE,
                            Cr_store = device.STORE_PLACE.CR_STORE,
                            device_status_code = device.DEVICE_STATUS.CODE,
                            Device_status_description = device.DEVICE_STATUS.DESCRIPTION,
                            Device_type_code = device.DEVICE_TYPE.CODE,
                            Number = device.DEVICE_NUMBER.NUMBER,
                            Mac_address = device.MAC_ADDRESS,
                            Ip = device.IP,
                            Processor = device.PROCESSOR,
                            Network_card = device.NETWORK_CARD,
                            Is_server = device.IS_SERVER,
                            Is_active = device.IS_ACTIVE,
                            Created_datetime = device.CREATED_DATETIME,
                            Created_by_operator_id = device.CREATED_BY_OPERATOR_ID,
                            Modified_datetime = device.MODIFIED_DATETIME,
                            Modified_by_operator_id = device.MODIFIED_BY_OPERATOR_ID,
                            Lcount = device.LCOUNT
                        }).ToList();
        }

        /// <summary>
        /// This function is responsible that validate if exists devices
        /// </summary>
        /// <param name="request"></param>        
        /// <returns>bool</returns>
        public async Task<bool> ValidateIfExistsDevices(GetDevicesQuery request)
        {
            return await Context.DEVICE.AsNoTracking()
                     .Include(device => device.DEVICE_NUMBER).AsNoTracking()
                     .Include(device => device.STORE_PLACE).AsNoTracking()
                     .Include(device => device.DEVICE_STATUS).AsNoTracking()
                     .Include(device => device.DEVICE_TYPE).AsNoTracking()
                     .Where(device => device.STORE_PLACE.IS_ACTIVE
                         && device.DEVICE_NUMBER.IS_ACTIVE
                         && (!string.IsNullOrEmpty(request.DeviceIdentifier) ? device.DEVICE_ID.ToString() == request.DeviceIdentifier : true))
                     .Skip((request.PageNumber - 1) * request.ItemsNumber).Take(request.ItemsNumber).AnyAsync();
        }

    }
}

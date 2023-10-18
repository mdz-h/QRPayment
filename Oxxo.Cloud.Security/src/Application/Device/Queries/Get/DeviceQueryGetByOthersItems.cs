using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxxo.Cloud.Security.Application.Device.Queries.Get
{
    public class DeviceQueryGetByOthersItems:IDeviceQueryGetByOthersItems
    {
        private readonly IApplicationDbContext Context;

        /// <summary>
        /// Constructor that injects the database context.  
        /// </summary>
        /// <param name="context">Context database</param>        
        public DeviceQueryGetByOthersItems(IApplicationDbContext context)
        {
            this.Context = context;
        }

        public async Task<List<DeviceResponseByOthersItems>> GetDevices(GetDevicesQueryByOthersItems request)
        {
            

                return (await Context.DEVICE.AsNoTracking()
                        .Include(deviceforothersitems => deviceforothersitems.DEVICE_NUMBER).AsNoTracking()
                        .Include(deviceforothersitems => deviceforothersitems.STORE_PLACE).AsNoTracking()
                        .Include(deviceforothersitems => deviceforothersitems.DEVICE_STATUS).AsNoTracking()
                        .Include(deviceforothersitems => deviceforothersitems.DEVICE_TYPE).AsNoTracking()
                        .Where(deviceforothersitems => deviceforothersitems.STORE_PLACE.IS_ACTIVE
                            && deviceforothersitems.DEVICE_NUMBER.IS_ACTIVE
                            && deviceforothersitems.DEVICE_NUMBER.DEVICE_NUMBER_ID == Convert.ToInt32(request.CajaId)
                            && deviceforothersitems.STORE_PLACE.CR_STORE == request.CrTienda && deviceforothersitems.STORE_PLACE.CR_PLACE == request.CrPlaza
                            && (!string.IsNullOrEmpty(request.DeviceIdentifier) ? deviceforothersitems.DEVICE_ID.ToString() == request.DeviceIdentifier : true)
                            )
                        .Skip((request.PageNumber - 1) * request.ItemsNumber).Take(request.ItemsNumber).ToListAsync()
                        ).Select(deviceforothersitems =>
                            new DeviceResponseByOthersItems()
                            {
                                Device_id = deviceforothersitems.DEVICE_ID,
                                Name = deviceforothersitems.NAME,
                                Cr_place = deviceforothersitems.STORE_PLACE.CR_PLACE,
                                Cr_store = deviceforothersitems.STORE_PLACE.CR_STORE,
                                device_status_code = deviceforothersitems.DEVICE_STATUS.CODE,
                                Device_status_description = deviceforothersitems.DEVICE_STATUS.DESCRIPTION,
                                Device_type_code = deviceforothersitems.DEVICE_TYPE.CODE,
                                Number = deviceforothersitems.DEVICE_NUMBER.NUMBER,
                                Mac_address = deviceforothersitems.MAC_ADDRESS,
                                Ip = deviceforothersitems.IP,
                                Processor = deviceforothersitems.PROCESSOR,
                                Network_card = deviceforothersitems.NETWORK_CARD,
                                Is_server = deviceforothersitems.IS_SERVER,
                                Is_active = deviceforothersitems.IS_ACTIVE,
                                Created_datetime = deviceforothersitems.CREATED_DATETIME,
                                Created_by_operator_id = deviceforothersitems.CREATED_BY_OPERATOR_ID,
                                Modified_datetime = deviceforothersitems.MODIFIED_DATETIME,
                                Modified_by_operator_id = deviceforothersitems.MODIFIED_BY_OPERATOR_ID,
                                Lcount = deviceforothersitems.LCOUNT
                            }).ToList();
            
        }

        public async Task<bool> ValidateIfExistsDevices(GetDevicesQueryByOthersItems request)
        {
            //return await Context.DEVICE.AsNoTracking()
            //        .Include(deviceforothersitems => deviceforothersitems.DEVICE_NUMBER).AsNoTracking()
            //        .Include(deviceforothersitems => deviceforothersitems.STORE_PLACE).AsNoTracking()
            //        .Include(deviceforothersitems => deviceforothersitems.DEVICE_STATUS).AsNoTracking()
            //        .Include(deviceforothersitems => deviceforothersitems.DEVICE_TYPE).AsNoTracking()
            //        .Where(deviceforothersitems => deviceforothersitems.STORE_PLACE.IS_ACTIVE
            //            && deviceforothersitems.DEVICE_NUMBER.IS_ACTIVE                        
            //            && (!string.IsNullOrEmpty(request.CrPlaza) ? deviceforothersitems.DEVICE_NUMBER_ID == request.ItemsNumber : true))
            //            //&& (!string.IsNullOrEmpty(request.CrTienda) ? deviceforothersitems.CR_TIENDA.ToString() == request.CrTienda : true)
            //            //&& (!string.IsNullOrEmpty(request.CajaId) ? deviceforothersitems.ID_CAJA.ToString() == request.CajaId : true))
            //        .Skip((request.PageNumber - 1) * request.ItemsNumber).Take(request.ItemsNumber).AnyAsync();

            return await Context.DEVICE.AsNoTracking()
           .Include(deviceforothersitems => deviceforothersitems.DEVICE_NUMBER).AsNoTracking()
           .Include(deviceforothersitems => deviceforothersitems.STORE_PLACE).AsNoTracking()
           .Include(deviceforothersitems => deviceforothersitems.DEVICE_STATUS).AsNoTracking()
           .Include(deviceforothersitems => deviceforothersitems.DEVICE_TYPE).AsNoTracking()
           .Where(deviceforothersitems => deviceforothersitems.STORE_PLACE.IS_ACTIVE
               && deviceforothersitems.DEVICE_NUMBER.IS_ACTIVE
               && deviceforothersitems.DEVICE_NUMBER.DEVICE_NUMBER_ID == Convert.ToInt32(request.CajaId)
                            && deviceforothersitems.STORE_PLACE.CR_STORE == request.CrTienda && deviceforothersitems.STORE_PLACE.CR_PLACE == request.CrPlaza
               && (!string.IsNullOrEmpty(request.DeviceIdentifier) ? deviceforothersitems.DEVICE_ID.ToString() == request.DeviceIdentifier : true))
           .Skip((request.PageNumber - 1) * request.ItemsNumber).Take(request.ItemsNumber).AnyAsync();
        }
    }
}

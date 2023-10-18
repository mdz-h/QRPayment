using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Application.Device.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces
{
    public interface IDeviceQueryGetByOthersItems
    {
        Task<List<DeviceResponseByOthersItems>> GetDevices(GetDevicesQueryByOthersItems request);
        Task<bool> ValidateIfExistsDevices(GetDevicesQueryByOthersItems request);
    }
}

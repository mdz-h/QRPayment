#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-11-26.
// Comment: Interface IDeviceQuery
//===============================================================================
#endregion
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Application.Device.Queries;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces
{
    public interface IDeviceQueryGet
    {
        Task<List<DeviceResponse>> GetDevices(GetDevicesQuery request);
        Task<bool> ValidateIfExistsDevices(GetDevicesQuery request);
    }
}

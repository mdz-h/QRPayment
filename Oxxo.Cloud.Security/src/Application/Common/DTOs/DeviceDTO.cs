#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Dto device.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Application.Common.DTOs;
public class DeviceDto : AuthenticateDto
{
    public DeviceDto() { 
        Id = string.Empty;
        DeviceKey= string.Empty;
    }

    public string Id { get; set; }
    public string DeviceKey { get; set; }
}

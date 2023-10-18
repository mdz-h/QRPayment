#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Dto device token.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Application.Common.DTOs;
public class DeviceTokenDto
{
    public DeviceTokenDto() {
        Key = string.Empty;
        CrPlace = string.Empty;
        CrStore = string.Empty;
        MacAddress = string.Empty;
        IP = string.Empty;
        IdProcessor = string.Empty;
        NetworkCard = string.Empty;
        TimeTokenExpiration = string.Empty;
        Name = string.Empty;
    }
    public Guid DeviceId { get; set; }
    public string Key { get; set; }
    public string CrPlace { get; set; }
    public string CrStore { get; set; }
    public int NumberDevice { get; set; }
    public string MacAddress { get; set; }
    public string IP { get; set; }
    public string IdProcessor { get; set; }
    public string NetworkCard { get; set; }
    public string TimeTokenExpiration { get; set; }
    public string Name { get; set; }
}

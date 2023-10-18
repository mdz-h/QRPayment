#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie device number.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Domain.Entities;
public class DeviceNumber : BaseEntity
{
    public DeviceNumber()
    {
        DEVICES = new List<Device>();
    }
    public int DEVICE_NUMBER_ID { get; set; }
    public int NUMBER { get; set; }
    public string? CODE { get; set; }
    public string? DESCRIPTION { get; set; }
    public bool IS_ACTIVE { get; set; }
    public int LCOUNT { get; set; }
    public Guid CREATED_BY_OPERATOR_ID { get; set; }
    public DateTime CREATED_DATETIME { get; set; }
    public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
    public DateTime? MODIFIED_DATETIME { get; set; }
    public List<Device> DEVICES { get; set; }

}

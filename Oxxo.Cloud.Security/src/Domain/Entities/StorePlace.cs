#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie store.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Domain.Entities;
public class StorePlace : BaseEntity
{
    public StorePlace()
    {
        DEVICES = new List<Device>();
        SYSTEM_PARAM = new List<SystemParam>();
    }
    public int STORE_PLACE_ID { get; set; }
    public int STORE_ID_SOURCE { get; set; }
    public int PLACE_ID_SOURCE { get; set; }
    public string CR_STORE { get; set; } = string.Empty;
    public string CR_PLACE { get; set; } = string.Empty;
    public bool IS_ACTIVE { get; set; }
    public int LCOUNT { get; set; }
    public Guid CREATED_BY_OPERATOR_ID { get; set; }
    public DateTime CREATED_DATETIME { get; set; }
    public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
    public DateTime? MODIFIED_DATETIME { get; set; }
    public List<Device> DEVICES { get; set; }
    public List<SystemParam> SYSTEM_PARAM { get; set; }
    public List<OperatorStoreLink> OPERATORSTORELINK { get; set; } = null!;
}

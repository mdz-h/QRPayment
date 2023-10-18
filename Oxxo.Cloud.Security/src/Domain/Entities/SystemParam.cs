#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie systema param value.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Domain.Entities;
public class SystemParam : BaseEntity
{
    public int SYSTEM_PARAM_ID { get; set; }
    public int SYSTEM_PARAM_ID_SOURCE { get; set; }
    public int? SYSTEM_PARAM_VALUE_ID_SOURCE { get; set; }
    public int? STORE_PLACE_ID { get; set; }
    public StorePlace? STORE_PLACE { get; set; }
    public string? CR_STORE { get; set; }
    public string? CR_PLACE { get; set; }
    public string? CODE_PARAM_TYPE { get; set; }
    public string? PARAM_CODE { get; set; }
    public string? CODE_PARAM_VALUE_TYPE { get; set; }
    public string? PARAM_VALUE { get; set; }
    public bool IS_ACTIVE { get; set; }
    public int LCOUNT { get; set; }
    public Guid CREATED_BY_OPERATOR_ID { get; set; }
    public DateTime CREATED_DATETIME { get; set; }
    public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
    public DateTime? MODIFIED_DATETIME { get; set; }
}

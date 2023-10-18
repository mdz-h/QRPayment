#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie external application.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Domain.Entities;
public class ExternalApplication
{
    public Guid EXTERNAL_APPLICATION_ID { get; set; }
    public string CODE { get; set; } = string.Empty;
    public string NAME { get; set; } = string.Empty;
    public int TIME_EXPIRATION_TOKEN { get; set; }
    public bool IS_ACTIVE { get; set; }
    public int LCOUNT { get; set; }
    public Guid CREATED_BY_OPERATOR_ID { get; set; }
    public DateTime CREATED_DATETIME { get; set; }
    public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
    public DateTime? MODIFIED_DATETIME { get; set; }
    public List<ApiKey> APIKEYS { get; set; } = new List<ApiKey>();
}

#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie session token.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Domain.Entities;
public class SessionToken : BaseEntity
{
    public int SESSION_TOKEN_ID { get; set; }
    public Guid GUID { get; set; }
    public int SESSION_STATUS_ID { get; set; }
    public string? TOKEN { get; set; }
    public string? REFRESH_TOKEN { get; set; }
    public DateTime EXPIRATION_TOKEN { get; set; }
    public DateTime START_DATETIME { get; set; }
    public DateTime END_DATETIME { get; set; }
    public bool IS_ACTIVE { get; set; }
    public int LCOUNT { get; set; }
    public Guid CREATED_BY_OPERATOR_ID { get; set; }
    public DateTime CREATED_DATETIME { get; set; }
    public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
    public DateTime? MODIFIED_DATETIME { get; set; }

    public List<OperatorStoreLink> OPERATORSTORELINKS { get; set; } = null!;
}
#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie operator.
//===============================================================================
#endregion

namespace Oxxo.Cloud.Security.Domain.Entities;
public class Operator : BaseEntity
{
    public Guid OPERATOR_ID { get; set; }
    public int PERSON_ID { get; set; }
    public int OPERATOR_STATUS_ID { get; set; }
    public string USER_NAME { get; set; } = string.Empty;
    public bool FL_INTRN { get; set; }
    public bool IS_ACTIVE { get; set; }
    public int LCOUNT { get; set; }
    public Guid CREATED_BY_OPERATOR_ID { get; set; }
    public DateTime CREATED_DATETIME { get; set; }
    public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
    public DateTime? MODIFIED_DATETIME { get; set; }
    public Person PERSON { get; set; }
    public OperatorStatus OPERATOR_STATUS { get; set; }
    public List<OperatorPassword> OPERATOR_PASSWORD { get; set; } = new List<OperatorPassword>();
}

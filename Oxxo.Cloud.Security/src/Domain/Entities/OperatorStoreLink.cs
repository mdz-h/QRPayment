#region File Information
//===============================================================================
// Author:  OMAR TORIBIO MORALES FLORES (NEORIS).
// Date:    06/12/2022.
// Comment: Operator Stor eLink.
//===============================================================================
#endregion

namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class OperatorStoreLink : BaseEntity
    {
        public int OPERATOR_STORE_LINK_ID { get; set; }
        public Guid OPERATOR_ID { get; set; }
        public int STORE_PLACE_ID { get; set; }     
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
        public int SESSION_TOKEN_ID { get; set; }
        public StorePlace STOREPLACE { get; set; } = null!;
        public SessionToken SESSIONTOKEN { get; set; } = null!;
    }
}

namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class OperatorPassword : BaseEntity
    {
        public int OPERATOR_PASSWORD_ID { get; set; }
        public Guid OPERATOR_ID { get; set; }
        public string PASSWORD { get; set; } = string.Empty;
        public DateTime EXPIRATION_TIME { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
        public Operator? OPERATOR { get; set; }
    }
}

namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class ValidIPRange : BaseEntity
    {
        public int VALID_IP_RANGE_IP { get; set; }
        public string? IP_RANGE { get; set; }
        public string? DESCRIPTION { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
    }
}

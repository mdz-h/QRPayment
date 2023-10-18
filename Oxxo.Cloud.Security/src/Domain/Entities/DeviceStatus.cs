namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class DeviceStatus : BaseEntity
    {
        public int DEVICE_STATUS_ID { get; set; }
        public string? CODE { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
        public List<Device>? DEVICES { get; set; }

    }
}

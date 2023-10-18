using System.Text.Json.Serialization;

namespace Oxxo.Cloud.Security.Application.Common.Models
{
    public class HistoryInvResponse
    {
        [JsonPropertyName("logBookId")]
        public Guid LOGBOOK_ID { get; set; }
        [JsonPropertyName("storePlaceId")]
        public int STORE_PLACE_ID { get; set; }
        [JsonPropertyName("initDate")]
        public DateTime INIT_DATE { get; set; }
        [JsonPropertyName("inventoryStatusId")]
        public string? INVENTORY_STATUS_ID { get; set; }
        [JsonPropertyName("InventoryTypeId")]
        public string? INVENTORY_TYPE_ID { get; set; }
        [JsonPropertyName("advanceStatusId")]
        public string? ADVANCE_STATUS_ID { get; set; }
        [JsonPropertyName("isConsolidated")]
        public bool? IS_CONSOLIDATED { get; set; }
        [JsonPropertyName("businessDate")]
        public DateTime BUSINESS_DATE { get; set; }
        [JsonPropertyName("isAfected")]
        public bool? IS_AFECTED { get; set; }
        [JsonPropertyName("auditor")]
        public int? AUDITOR { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime? END_DATE { get; set; }
        [JsonPropertyName("aditionalNotes")]
        public string? ADDITIONAL_NOTES { get; set; }
        [JsonPropertyName("preAnalysisDate")]
        public DateTime? PRE_ANALYSIS_DATE { get; set; }
        [JsonPropertyName("isActive")]
        public bool IS_ACTIVE { get; set; }
    }
}

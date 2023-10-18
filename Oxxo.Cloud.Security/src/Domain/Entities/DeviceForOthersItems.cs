using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxxo.Cloud.Security.Domain.Entities
{
    public class DeviceForOthersItems : BaseEntity
    {
        [Key]
        public Guid DEVICE_ID { get; set; } 
        public int STORE_PLACE_ID { get; set; }
        public StorePlace STORE_PLACE { get; set; }
        public int DEVICE_TYPE_ID { get; set; }
        public DeviceType DEVICE_TYPE { get; set; }
        public int DEVICE_STATUS_ID { get; set; }
        public DeviceStatus DEVICE_STATUS { get; set; }
        public int DEVICE_NUMBER_ID { get; set; }
        public DeviceNumber DEVICE_NUMBER { get; set; }
        public string MAC_ADDRESS { get; set; } = string.Empty;
        public string IP { get; set; } = string.Empty;
        public string PROCESSOR { get; set; } = string.Empty;
        public string NETWORK_CARD { get; set; } = string.Empty;
        public string NAME { get; set; } = string.Empty;
        public string DESCRIPTION { get; set; } = string.Empty;
        public bool IS_SERVER { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int LCOUNT { get; set; }
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
    }
}

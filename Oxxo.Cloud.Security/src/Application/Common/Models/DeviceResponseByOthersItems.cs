using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxxo.Cloud.Security.Application.Common.Models
{
    public class DeviceResponseByOthersItems
    {
        public Guid Device_id { get; set; }
        public string? Name { get; set; }
        public string? Cr_place { get; set; }
        public string? Cr_store { get; set; }
        public string? device_status_code { get; set; }
        public string? Device_status_description { get; set; }
        public string? Device_type_code { get; set; }
        public int Number { get; set; }
        public string? Mac_address { get; set; }
        public string? Ip { get; set; }
        public string? Processor { get; set; }
        public string? Network_card { get; set; }
        public Boolean Is_server { get; set; }
        public Boolean Is_active { get; set; }
        public DateTime Created_datetime { get; set; }
        public Guid Created_by_operator_id { get; set; }
        public DateTime? Modified_datetime { get; set; }
        public Guid? Modified_by_operator_id { get; set; }
        public int Lcount { get; set; }
    }
}

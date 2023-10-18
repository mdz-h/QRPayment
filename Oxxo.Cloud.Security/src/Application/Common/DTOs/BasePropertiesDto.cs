using System.Runtime.Serialization;

namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    public class BasePropertiesDto
    {
        public BasePropertiesDto()
        {
            Source = string.Empty;
            BearerToken=string.Empty;
            EndPoint=string.Empty;  
        }
        [IgnoreDataMember]
        public string Source { get; set; }
        [IgnoreDataMember]
        public string BearerToken { get; set; }

        public string Token
        {
            get { return (BearerToken ?? string.Empty).Replace("Bearer ", string.Empty); }
        }
        [IgnoreDataMember]
        public string EndPoint { get; set; }
        [IgnoreDataMember]
        public string UserIdentification { get; set; } = string.Empty;
        [IgnoreDataMember]
        public string Identification { get; set; } = string.Empty;
        public string FullName { get; set; }
    }
}

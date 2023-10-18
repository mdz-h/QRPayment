using System.Runtime.Serialization;

namespace Oxxo.Cloud.Security.Application.Common.Models
{
    public class BaseProperties
    {
        public BaseProperties()
        {
            UserIdentification = string.Empty;
        }
        [IgnoreDataMember]
        public string UserIdentification { get; set; }
    }
}

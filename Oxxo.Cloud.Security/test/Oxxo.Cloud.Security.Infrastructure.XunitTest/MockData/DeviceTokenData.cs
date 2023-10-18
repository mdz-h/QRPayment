using Oxxo.Cloud.Security.Application.Common.DTOs;
using System.Collections;

namespace Oxxo.Cloud.Security.Infrastructure.MockData
{
    public class DeviceTokenData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new DeviceTokenDto
                {
                  Key = "Code",
                  CrPlace = "Place",
                  CrStore = "Store",
                  TimeTokenExpiration = "2",
                  NumberDevice = 2,
                  MacAddress = "MacAddres",
                  IP = "IP",
                  IdProcessor = "IdProcessor",
                  NetworkCard = "NetworkCard",
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Enum;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces
{
    public interface IExternalService
    {
        public Task<GenResponse<T>> CallToExternalApiAsync<T>(string endpoint, string tokenAuth, RestMethodEnum method, T? paramsContent = default);
    }
}

using Oxxo.Cloud.Logging.Domain.Enums;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces
{
    public interface ILogService
    {
        public Task<bool> Logger(string eventMethod, string method, LogTypeEnum logType, string userIdentification, string message, string source);
        public Task<bool> Logger(string eventMethod, string method, LogTypeEnum logType, string message, string source);
    }
}

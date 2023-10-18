using Oxxo.Cloud.Logging.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces
{
    public interface ILogService2
    {
        public Task<bool> Logger(string eventMethod, string method, LogTypeEnum logType, string userIdentification, string message, string source);
        public Task<bool> Logger(string eventMethod, string method, LogTypeEnum logType, string message, string source);
    }
}

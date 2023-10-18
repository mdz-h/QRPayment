using Oxxo.Cloud.Logging.Application.Logger.Commands.CreateLog;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Logging.Infraestructure.Persistence;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;

namespace Oxxo.Cloud.Security.Infrastructure.Services
{
    public class LogService : ILogService
    {
        /// <summary>
        /// This method sends the logs to an external service
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventType"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> Logger(string eventMethod, string method, LogTypeEnum logType, string message, string source)
        {
            try
            {
                var log = new Log(new ServiceBus());
                await log.WriteLog(eventMethod, method, logType, message, string.Empty, GlobalConstantHelpers.LOG_NAME_MICROSERVICE, source, false);
                Console.WriteLine(string.Format("[{0}] EventMethod: {1}, Method: {2}, Message: {3}, Component: {4}, Source: {5}",
                                                logType.ToString(), eventMethod, method, message, GlobalConstantHelpers.LOG_NAME_MICROSERVICE, source));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("LogService: " + ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> Logger(string eventMethod, string method, LogTypeEnum logType, string userIdentification, string message, string source)
        {
            try
            {
                var log = new Log(new ServiceBus());
                await log.WriteLog(eventMethod, method, logType, message, userIdentification, GlobalConstantHelpers.LOG_NAME_MICROSERVICE, source, false);
                Console.WriteLine(string.Format("[{0}] EventMethod: {1}, Method: {2}, Message: {3}, Component: {4}, Source: {5}",
                                                logType.ToString(), eventMethod, method, message, GlobalConstantHelpers.LOG_NAME_MICROSERVICE, source));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("LogService: " + ex.StackTrace);
                return false;
            }
        }
    }
}
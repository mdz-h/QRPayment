using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Diagnostics;
using System.Text.Json;

namespace Oxxo.Cloud.Security.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogService logService;
        public LoggingBehaviour(ILogService logService)
        {
            this.logService = logService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            dynamic baseProperties = request;
            string userIdentification = baseProperties.UserIdentification;
            string requestName = request.GetType().Name;
            string requestHandler = $"{requestName}.Handler";
            string requestClass = $"{requestName}.cs";
            string requestFullNamespace = request.GetType().Namespace?.Split(".").Last() ?? string.Empty;

            TResponse response;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                try
                {
                    await logService.Logger(requestFullNamespace,
                        requestHandler,
                        LogTypeEnum.Information,
                         userIdentification,
                        $"{GlobalConstantMessages.STARTEDCOMMANDBEHAVIOURS} {requestName} {GlobalConstantMessages.WITHDATABEHAVIOURS}: {JsonSerializer.Serialize(request)}",
                        requestClass);
                }
                catch (NotSupportedException)
                {
                    await logService.Logger(requestFullNamespace,
                        requestHandler,
                        LogTypeEnum.Information,
                        userIdentification,
                        $"{GlobalConstantMessages.STARTEDCOMMANDBEHAVIOURS} {requestName}",
                        requestClass);
                }
                response = await next();
            }
            finally
            {
                stopwatch.Stop();
                await logService.Logger(requestFullNamespace,
                        requestHandler,
                        LogTypeEnum.Information,
                        userIdentification,
                        $"{GlobalConstantMessages.FINISHEDCOMMANDBEHAVIOURS} {requestName} {GlobalConstantMessages.EXECUTIONBEHAVIOURS}: {stopwatch.ElapsedMilliseconds}",
                        requestClass);
            }

            return response;
        }
    }
}

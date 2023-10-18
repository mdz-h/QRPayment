using System.Net;

namespace Oxxo.Cloud.Security.Application.Common.Models
{
    public class GenResponse<T>
    {
        public HttpStatusCode StatusCode { get; }
        public IEnumerable<string>? Errors { get; }
        public T? Body { get; }

        public GenResponse(HttpStatusCode statusCode, T? body, IEnumerable<string>? errors = null)
        {
            StatusCode = statusCode;
            Errors = errors;
            Body = body;
        }

        public GenResponse(HttpStatusCode statusCode, IEnumerable<string>? errors = null)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}

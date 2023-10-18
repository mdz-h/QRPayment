namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    public class RequestExternalCommDto
    {
        public RequestExternalCommDto()
        {
            Service = string.Empty;
            Timeout = string.Empty;
            HeaderParams = null;
            PayloadType = string.Empty;
            Payload = new();
            BarrerToken = string.Empty;
        }

        public string Service { get; set; }
        public string Timeout { get; set; }
        public List<HeaderParamDto>? HeaderParams { get; set; }
        public string PayloadType { get; set; }
        public PayloadDto Payload { get; set; }
        public string? BarrerToken { get; set; }

    }
    public class HeaderParamDto
    {
        public HeaderParamDto()
        {
            Key = string.Empty;
            Value = string.Empty;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class PayloadDto
    {
        public PayloadDto()
        {
            Method = string.Empty;
            ContentType = string.Empty;
            QueryParams = new List<QueryParamDto> { };
        }

        public string Method { get; set; }
        public string ContentType { get; set; }
        public List<QueryParamDto> QueryParams { get; set; }
    }

    public class QueryParamDto
    {
        public QueryParamDto()
        {
            Key = string.Empty;
            Value = string.Empty;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

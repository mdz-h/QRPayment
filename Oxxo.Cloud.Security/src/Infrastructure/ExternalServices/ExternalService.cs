using Newtonsoft.Json;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Enum;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Oxxo.Cloud.Security.Infrastructure.ExternalServices
{
    public class ExternalService : IExternalService
    {
        private readonly HttpClient _httpClient;
        private string _endpoint;
        private string _tokenAuth;
        HttpResponseMessage? response;

        public ExternalService()
        {
            _httpClient = new HttpClient();
            response = new HttpResponseMessage();
            _endpoint = string.Empty;
            _tokenAuth = string.Empty;
        }

        public async Task<GenResponse<T>> CallToExternalApiAsync<T>(string endpoint, string tokenAuth, RestMethodEnum method, T? paramsContent)
        {
            try
            {
                if (this._endpoint == null)
                {
                    return new GenResponse<T>(HttpStatusCode.BadRequest, new List<string>()
                    {
                        "Missing enpoint"
                    });
                }
                this._endpoint = endpoint;
                this._tokenAuth = tokenAuth;
                T? data = default;
                if (!ValidateRequest())
                {
                    return new GenResponse<T>(HttpStatusCode.Unauthorized, new List<string>()
                    {
                        "Missing token"
                    });
                }

                StringContent? content = default;
                if (paramsContent != null)
                {
                    content = new(JsonConvert.SerializeObject(paramsContent), Encoding.UTF8, "application/json");
                }
                else
                {
                    if (method != RestMethodEnum.GET || method == RestMethodEnum.DELETE)
                    {
                        return new GenResponse<T>(HttpStatusCode.BadRequest, new List<string>()
                    {
                        "Missing parameters"
                    });
                    }
                }

                response = await MakeRequest(method, content);
                response.EnsureSuccessStatusCode();

                ValidateResponse(response, out data);

                if (data == null)
                {
                    return new GenResponse<T>(HttpStatusCode.NoContent);
                }

                return new GenResponse<T>(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return new GenResponse<T>(HttpStatusCode.InternalServerError, new List<string>()
                {
                    ex.Message
                });
            }
        }


        public async Task<GenResponse<T>> CallToPostExternalApiAsync<T>(string endpoint, string tokenAuth, T paramsContent)
        {
            try
            {
                this._endpoint = endpoint;
                this._tokenAuth = tokenAuth;
                T? data = default;
                if (!ValidateRequest())
                {
                    return new GenResponse<T>(HttpStatusCode.Unauthorized, null);
                }
                StringContent content;
                if (paramsContent != null)
                {
                    content = new(JsonConvert.SerializeObject(paramsContent), Encoding.UTF8, "application/json");
                }
                else
                {
                    return new GenResponse<T>(HttpStatusCode.BadRequest, null);
                }

                response = await _httpClient.PostAsync(_endpoint, content);
                response.EnsureSuccessStatusCode();

                ValidateResponse(response, out data);

                if (data == null)
                {
                    return new GenResponse<T>(HttpStatusCode.NoContent, null);
                }

                return new GenResponse<T>(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool ValidateRequest()
        {
            if (!string.IsNullOrEmpty(_tokenAuth))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenAuth);
            }
            else
            {
                return false;
            }
            return true;
        }

        private async Task<HttpResponseMessage> MakeRequest(RestMethodEnum method, StringContent? paramsContent)
        {
            HttpResponseMessage? responseMessage = null;
            switch (method)
            {
                case RestMethodEnum.GET:
                    responseMessage = await _httpClient.GetAsync(_endpoint);
                    break;
                case RestMethodEnum.POST:
                    responseMessage = await _httpClient.PostAsync(_endpoint, paramsContent);
                    break;
                case RestMethodEnum.PUT:
                    responseMessage = await _httpClient.PutAsync(_endpoint, paramsContent);
                    break;
                case RestMethodEnum.PATCH:
                    responseMessage = await _httpClient.PatchAsync(_endpoint, paramsContent);
                    break;
                case RestMethodEnum.DELETE:
                    responseMessage = await _httpClient.DeleteAsync(_endpoint);
                    break;
            }
            return responseMessage;

        }
        private void ValidateResponse<T>(HttpResponseMessage response, out T? data)
        {
            var resultResponse = response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (!string.IsNullOrEmpty(resultResponse.Result))
            {
                data = System.Text.Json.JsonSerializer.Deserialize<T>(resultResponse.Result, options);
            }
            else
            {
                data = default;
            }
        }
    }
}
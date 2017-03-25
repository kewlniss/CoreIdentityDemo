using CoreIdentityDemo.Common.Util;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoreIdentityDemo.ApiClients
{
    public abstract class BaseApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        public BaseApiClient(string baseAddress)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public void Dispose()
        {
            if (_httpClient != null)
                _httpClient.Dispose();
        }

        protected async Task<TResponse> GetAsync<TResponse>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            return await deserializeResponse<TResponse>(response);
        }

        protected TResponse Get<TResponse>(string requestUri)
        {
            return GetAsync<TResponse>(requestUri).Result;
        }

        protected async Task GetAsync(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            handleVoidResponse(response);
        }

        protected void Get(string requestUri)
        {
            GetAsync(requestUri).Wait();
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest requestObj)
        {
            var requestJson = JsonConvert.SerializeObject(requestObj);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUri, content);
            return await deserializeResponse<TResponse>(response);
        }

        protected TResponse Post<TRequest, TResponse>(string requestUri, TRequest requestObj)
        {
            return PostAsync<TRequest, TResponse>(requestUri, requestObj).Result;
        }

        protected async Task PostAsync<TRequest>(string requestUri, TRequest requestObj)
        {
            var requestJson = JsonConvert.SerializeObject(requestObj);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUri, content);
            handleVoidResponse(response);
        }

        protected void Post<TRequest>(string requestUri, TRequest requestObj)
        {
            PostAsync(requestUri, requestObj).Wait();
        }

        protected async Task<TResponse> DeleteAsync<TResponse>(string requestUri)
        {
            var response = await _httpClient.DeleteAsync(requestUri);
            return await deserializeResponse<TResponse>(response);
        }

        protected TResponse Delete<TResponse>(string requestUri)
        {
            return DeleteAsync<TResponse>(requestUri).Result;
        }

        protected async Task DeleteAsync(string requestUri)
        {
            var response = await _httpClient.DeleteAsync(requestUri);
            handleVoidResponse(response);
        }

        protected void Delete(string requestUri)
        {
            DeleteAsync(requestUri).Wait();
        }

        protected async Task<TResponse> PutAsync<TRequest, TResponse>(string requestUri, TRequest requestObj)
        {
            var requestJson = JsonConvert.SerializeObject(requestObj);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(requestUri, content);
            return await deserializeResponse<TResponse>(response);
        }

        protected TResponse Put<TRequest, TResponse>(string requestUri, TRequest requestObj)
        {
            return PutAsync<TRequest, TResponse>(requestUri, requestObj).Result;
        }

        protected async Task PutAsync<TRequest>(string requestUri, TRequest requestObj)
        {
            var requestJson = JsonConvert.SerializeObject(requestObj);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(requestUri, content);
            handleVoidResponse(response);
        }

        protected void Put<TRequest>(string requestUri, TRequest requestObj)
        {
            PutAsync(requestUri, requestObj).Wait();
        }

        #region Private Methods

        private async Task<T> deserializeResponse<T>(HttpResponseMessage response)
        {
            ExceptionUtil.ThrowIfNull(response, nameof(response));

            var responseContent = await tryReadAsString(response.Content);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return JsonConvert.DeserializeObject<T>(responseContent);
                case HttpStatusCode.NotFound:
                    return default(T);
                default:
                    var request = response.RequestMessage;
                    var requestContent = await tryReadAsString(request.Content);
                    throw new ApiException(request.RequestUri, request.Method, request.Headers,
                        requestContent, response.StatusCode, response.Headers, responseContent);
            }
        }

        private async void handleVoidResponse(HttpResponseMessage response)
        {
            ExceptionUtil.ThrowIfNull(response, nameof(response));

            if (response.StatusCode == HttpStatusCode.OK)
                return;

            var request = response.RequestMessage;
            var requestContent = await tryReadAsString(request.Content);
            var responseContent = await tryReadAsString(response.Content);

            throw new ApiException(request.RequestUri, request.Method, request.Headers,
                requestContent, response.StatusCode, response.Headers, responseContent);
        }

        private async Task<string> tryReadAsString(HttpContent content)
        {
            ExceptionUtil.ThrowIfNull(content, nameof(content));
            try
            {
                return await content.ReadAsStringAsync();
            }
            catch
            {
                return default(string);
            }
        }
        #endregion
    }
}

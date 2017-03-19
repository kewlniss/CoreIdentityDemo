using Newtonsoft.Json;
using System;
using System.Net.Http;
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
            var result = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
            return result;
        }

        protected TResponse Get<TResponse>(string requestUri)
        {
            return GetAsync<TResponse>(requestUri).Result;
        }

        protected async Task GetAsync(string requestUri)
        {
            await _httpClient.GetAsync(requestUri);
        }

        protected void Get(string requestUri)
        {
            GetAsync(requestUri).Wait();
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest requestObj)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(requestObj));
            var response = await _httpClient.PostAsync(requestUri, httpContent);
            var result = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
            return result;
        }

        protected TResponse Post<TRequest, TResponse>(string requestUri, TRequest requestObj)
        {
            return PostAsync<TRequest, TResponse>(requestUri, requestObj).Result;
        }

        protected async Task PostAsync<TRequest>(string requestUri, TRequest requestObj)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(requestObj));
            await _httpClient.PostAsync(requestUri, httpContent);
        }

        protected void Post<TRequest>(string requestUri, TRequest requestObj)
        {
            PostAsync(requestUri, requestObj).Wait();
        }

        protected async Task<TResponse> DeleteAsync<TResponse>(string requestUri)
        {
            var response = await _httpClient.DeleteAsync(requestUri);
            var result = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
            return result;
        }

        protected TResponse Delete<TResponse>(string requestUri)
        {
            return DeleteAsync<TResponse>(requestUri).Result;
        }

        protected async Task DeleteAsync(string requestUri)
        {
            await _httpClient.DeleteAsync(requestUri);
        }

        protected void Delete(string requestUri)
        {
            DeleteAsync(requestUri).Wait();
        }

        protected async Task<TResponse> PutAsync<TRequest, TResponse>(string requestUri, TRequest requestObj)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(requestObj));
            var response = await _httpClient.PutAsync(requestUri, httpContent);
            var result = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
            return result;
        }

        protected TResponse Put<TRequest, TResponse>(string requestUri, TRequest requestObj)
        {
            return PutAsync<TRequest, TResponse>(requestUri, requestObj).Result;
        }

        protected async Task PutAsync<TRequest>(string requestUri, TRequest requestObj)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(requestObj));
            await _httpClient.PutAsync(requestUri, httpContent);
        }

        protected void Put<TRequest>(string requestUri, TRequest requestObj)
        {
            PutAsync<TRequest>(requestUri, requestObj).Wait();
        }
    }
}

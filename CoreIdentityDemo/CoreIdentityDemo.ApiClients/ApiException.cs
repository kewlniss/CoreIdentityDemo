using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CoreIdentityDemo.ApiClients
{
    public class ApiException : Exception
    {
        public ApiException(Uri uri, HttpMethod method, HttpRequestHeaders requestHeaders,
            string requestContent, HttpStatusCode statusCode, HttpResponseHeaders responseHeaders,
            string responseContent)
            : base($"{method?.Method} {uri?.ToString()} {responseContent}")
        {
            Uri = uri;
            Method = method;
            RequestHeaders = requestHeaders;
            RequestContent = requestContent;
            StatusCode = statusCode;
            ResponseHeaders = responseHeaders;
            ResponseContent = responseContent;
        }

        public Uri Uri { get; private set; }
        public HttpMethod Method { get; private set; }
        public HttpRequestHeaders RequestHeaders { get; private set; }
        public string RequestContent { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public HttpResponseHeaders ResponseHeaders { get; private set; }
        public string ResponseContent { get; private set; }
    }
}

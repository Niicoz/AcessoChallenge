using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcessoChallenge.Infrastructure.Factories
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        public HttpClient HttpClient { get; }

        public HttpClientWrapper(HttpClient httpClient)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            var response = await HttpClient.GetAsync(requestUri);
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            var response = await HttpClient.PostAsync(requestUri, content);
            return response;
        }
    }
}
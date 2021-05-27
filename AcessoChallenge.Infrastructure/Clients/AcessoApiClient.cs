using AcessoChallenge.Infrastructure.Contracts;
using AcessoChallenge.Infrastructure.Factories;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AcessoChallenge.Infrastructure.Clients
{
    public class AcessoApiClient : IAcessoApiClient
    {
        public IHttpClientWrapper HttpClient { get; }
        public string AcessoUrl { get; }

        public AcessoApiClient(
            IHttpClientWrapper httpClient,
            string acessoUrl)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrEmpty(acessoUrl))
                throw new ArgumentNullException(nameof(acessoUrl));

            AcessoUrl = acessoUrl;
        }

        public async Task<HttpResponseMessage> CheckAccount(string accountId)
        {
            return await HttpClient.GetAsync($"{AcessoUrl}{accountId}");
        }

        public async Task<HttpResponseMessage> CreateEvent(Transfer transfer)
        {
            var requestJson = JsonSerializer.Serialize(transfer);

            var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            return await HttpClient.PostAsync(AcessoUrl, httpContent);
        }
    }
}
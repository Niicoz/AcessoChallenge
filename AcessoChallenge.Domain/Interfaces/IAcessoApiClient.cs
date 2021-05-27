using AcessoChallenge.Infrastructure.Contracts;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcessoChallenge.Infrastructure.Clients
{
    public interface IAcessoApiClient
    {
        Task<HttpResponseMessage> CheckAccount(string accountId);

        Task<HttpResponseMessage> CreateEvent(Transfer transfer);
    }
}
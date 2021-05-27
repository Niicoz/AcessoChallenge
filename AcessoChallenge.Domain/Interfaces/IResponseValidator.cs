using AcessoChallenge.Infrastructure.Contracts;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcessoChallenge.Domain.Interfaces
{
    public interface IResponseValidator
    {
        Task<ServiceResult<Account>> ResponseValidate(HttpResponseMessage response);
    }
}
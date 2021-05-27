using AcessoChallenge.Domain.Events;
using System.Threading.Tasks;

namespace AcessoChallenge.Domain.Interfaces
{
    public interface IAccountValidate
    {
        Task Validate(TransferSolicitationEvent solicitationEvent);
    }
}
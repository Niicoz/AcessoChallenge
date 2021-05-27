using AcessoChallenge.Domain.Events;
using System.Threading.Tasks;

namespace AcessoChallenge.Domain.Interfaces
{
    public interface ITransferProcess
    {
        Task Process(TransferEvent transferEvent);
    }
}
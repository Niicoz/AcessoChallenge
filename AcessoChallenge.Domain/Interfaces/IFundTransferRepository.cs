using AcessoChallenge.Domain.Entities;
using AcessoChallenge.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace AcessoChallenge.Domain.Interfaces
{
    public interface IFundTransferRepository
    {
        Task InsertFundTransferAsync(Guid transactionId);

        Task UpdateFundTransferStatusAsync(Guid transactionId, Status status);

        Task UpdateFundTransferStatusAndMessageAsync(Guid transactionId, Status status, string message);

        Task<FundTransferStatus> GetStatusByIdAsync(Guid transactionId);
    }
}
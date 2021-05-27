using AcessoChallenge.Domain.Entities;
using AcessoChallenge.Domain.Enums;
using AcessoChallenge.Domain.Interfaces;
using AcessoChallenge.Infrastructure.Factories;
using Dapper;
using System;
using System.Threading.Tasks;

namespace AcessoChallenge.Infrastructure.Repositories
{
    public class FundTransferRepository : IFundTransferRepository
    {
        public IDbConnectionFactory DbConnectionFactory { get; }

        public FundTransferRepository(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        public async Task InsertFundTransferAsync(Guid transactionId)
        {
            var status = Status.InQueue;

            using (var conn = DbConnectionFactory.GetConnection())
            {
                const string command = "INSERT INTO FundTransfer " +
                                       "(Id, Status) " +
                                       "VALUES (@transactionId, @status)";

                await conn.ExecuteAsync(command, new { transactionId, status });
            }
        }

        public async Task UpdateFundTransferStatusAsync(Guid transactionId, Status status)
        {
            using (var conn = DbConnectionFactory.GetConnection())
            {
                const string command = "UPDATE FundTransfer " +
                            "SET Status = @status " +
                            "WHERE Id = @transactionId";

                await conn.ExecuteAsync(command, new { transactionId, status });
            }
        }

        public async Task UpdateFundTransferStatusAndMessageAsync(Guid transactionId, Status status, string message)
        {
            using (var conn = DbConnectionFactory.GetConnection())
            {
                const string command = @"UPDATE FundTransfer
                            SET Status = @status,
                            Message = @message
                            WHERE Id = @transactionId";

                await conn.ExecuteAsync(command, new { transactionId, status, message });
            }
        }

        public async Task<FundTransferStatus> GetStatusByIdAsync(Guid transactionId)
        {
            using (var conn = DbConnectionFactory.GetConnection())
            {
                const string command = @"SELECT
                            Status, Message
                            FROM FundTransfer
                            WHERE Id = @transactionId";

                return await conn.QuerySingleOrDefaultAsync<FundTransferStatus>(command, new { transactionId });
            }
        }
    }
}
using AcessoChallenge.Domain.Enums;

namespace AcessoChallenge.Domain.Entities
{
    public class FundTransferStatus
    {
        public Status Status { get; }

        public string Message { get; set; }
    }
}
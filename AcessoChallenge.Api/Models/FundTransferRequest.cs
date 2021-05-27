using System;

namespace AcessoChallenge.Api.Models
{
    public class FundTransferRequest
    {
        public Guid TransactionId = Guid.NewGuid();

        public string AccountOrigin { get; set; }

        public string AccountDestination { get; set; }

        public float Value { get; set; }
    }
}
using System;

namespace AcessoChallenge.Domain.Events
{
    public interface TransferEvent
    {
        Guid TransactionId { get; }

        string AccountOrigin { get; }

        string AccountDestination { get; }

        float Value { get; }

        string Type { get; }
    }
}
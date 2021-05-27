using System;

namespace AcessoChallenge.Domain.Events
{
    public interface TransferSolicitationEvent
    {
        Guid TransactionId { get; }

        string AccountOrigin { get; }

        string AccountDestination { get; }

        float Value { get; }
    }
}
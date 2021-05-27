using AcessoChallenge.Domain.Events;
using AcessoChallenge.Domain.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace AcessoChallenge.Infrastructure.Consumers
{
    public class TransferEventConsumer : IConsumer<TransferEvent>
    {
        public ITransferProcess TransferProcess { get; }

        public TransferEventConsumer(ITransferProcess transferProcess)
        {
            TransferProcess = transferProcess ?? throw new ArgumentNullException(nameof(transferProcess));
        }

        public async Task Consume(ConsumeContext<TransferEvent> context)
        {
            await TransferProcess.Process(context.Message);
        }
    }
}
using AcessoChallenge.Domain.Events;
using AcessoChallenge.Domain.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace AcessoChallenge.Infrastructure.Consumers
{
    public class FundTransferValidateConsumer : IConsumer<TransferSolicitationEvent>
    {
        public IAccountValidate AccountValidate { get; }

        public FundTransferValidateConsumer(IAccountValidate accountValidate)
        {
            AccountValidate = accountValidate ?? throw new ArgumentNullException(nameof(accountValidate));
        }

        public async Task Consume(ConsumeContext<TransferSolicitationEvent> context)
        {
            await AccountValidate.Validate(context.Message);
        }
    }
}
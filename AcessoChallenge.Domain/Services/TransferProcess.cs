using AcessoChallenge.Domain.Enums;
using AcessoChallenge.Domain.Events;
using AcessoChallenge.Domain.Interfaces;
using AcessoChallenge.Infrastructure.Clients;
using AcessoChallenge.Infrastructure.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AcessoChallenge.Domain.Services
{
    public class TransferProcess : ITransferProcess
    {
        public IFundTransferRepository FundTransferRepository { get; }
        public IAcessoApiClient AcessoApiClient { get; }
        public IPublishEndpoint Publisher { get; }
        public IResponseValidator ResponseValidator { get; }
        public ILogger<TransferProcess> Logger { get; }

        public TransferProcess(
            IFundTransferRepository fundTransferRepository,
            IAcessoApiClient acessoApiClient,
            IPublishEndpoint publisher,
            IResponseValidator responseValidator,
            ILogger<TransferProcess> logger)
        {
            FundTransferRepository = fundTransferRepository ?? throw new ArgumentNullException(nameof(fundTransferRepository));
            AcessoApiClient = acessoApiClient ?? throw new ArgumentNullException(nameof(acessoApiClient));
            Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            ResponseValidator = responseValidator ?? throw new ArgumentNullException(nameof(responseValidator));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Process(TransferEvent transferEvent)
        {
            var transferModel = new Transfer(
                GetAccountByEventType(transferEvent),
                transferEvent.Value,
                transferEvent.Type);

            var transferResponse = await AcessoApiClient.CreateEvent(transferModel);

            var transferValidated = await ResponseValidator.ResponseValidate(transferResponse);

            if (!transferValidated.IsSucccess())
            {
                await FundTransferRepository.UpdateFundTransferStatusAndMessageAsync(
                    transferEvent.TransactionId,
                    Status.Error, transferValidated.Message);

                return;
            }

            Logger.LogInformation($"{transferModel.Type} of {transferModel.Value} in the number account {transferModel.AccountNumber}");

            if (transferEvent.Type == EventType.Debit.ToString())
            {
                await Publisher.Publish<TransferEvent>(new
                {
                    transferEvent.TransactionId,
                    transferEvent.AccountOrigin,
                    transferEvent.AccountDestination,
                    transferEvent.Value,
                    Type = EventType.Credit.ToString()
                });
            }
            else await FundTransferRepository.UpdateFundTransferStatusAsync(
                transferEvent.TransactionId,
                Status.Confirmed);
        }

        private static string GetAccountByEventType(TransferEvent transferEvent)
            => transferEvent.Type == EventType.Debit.ToString()
                ? transferEvent.AccountOrigin : transferEvent.AccountDestination;
    }
}
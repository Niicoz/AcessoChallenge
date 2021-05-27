using AcessoChallenge.Domain.Enums;
using AcessoChallenge.Domain.Events;
using AcessoChallenge.Domain.Interfaces;
using AcessoChallenge.Infrastructure.Clients;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace AcessoChallenge.Domain.Services
{
    public class AccountValidate : IAccountValidate
    {
        public IAcessoApiClient AcessoApiClient { get; }
        public IFundTransferRepository FundTransferRepository { get; }
        public IPublishEndpoint Publisher { get; }
        public IResponseValidator ResponseValidator { get; }

        public AccountValidate(
            IAcessoApiClient acessoApiClient,
            IFundTransferRepository fundTransferRepository,
            IPublishEndpoint publisher,
            IResponseValidator responseValidator)
        {
            AcessoApiClient = acessoApiClient ?? throw new ArgumentNullException(nameof(acessoApiClient));
            FundTransferRepository = fundTransferRepository ?? throw new ArgumentNullException(nameof(fundTransferRepository));
            Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            ResponseValidator = responseValidator ?? throw new ArgumentNullException(nameof(responseValidator));
        }

        public async Task Validate(TransferSolicitationEvent solicitationEvent)
        {
            var accountOriginResponse = await AcessoApiClient.CheckAccount(solicitationEvent.AccountOrigin);

            var accountOriginValidation = await ResponseValidator.ResponseValidate(accountOriginResponse);

            if (!accountOriginValidation.IsSucccess())
            {
                await FundTransferRepository.UpdateFundTransferStatusAndMessageAsync(
                    solicitationEvent.TransactionId, Status.Error, accountOriginValidation.Message);
                return;
            }

            var accountDestinationResponse = await AcessoApiClient.CheckAccount(solicitationEvent.AccountDestination);

            var accountDestinationValidation = await ResponseValidator.ResponseValidate(accountDestinationResponse);

            if (!accountDestinationValidation.IsSucccess())
            {
                await FundTransferRepository.UpdateFundTransferStatusAndMessageAsync(
                    solicitationEvent.TransactionId, Status.Error, accountDestinationValidation.Message);
                return;
            }

            await FundTransferRepository.UpdateFundTransferStatusAsync(
                solicitationEvent.TransactionId, Status.Processing);

            await Publisher.Publish<TransferEvent>(new
            {
                solicitationEvent.TransactionId,
                solicitationEvent.AccountOrigin,
                solicitationEvent.AccountDestination,
                solicitationEvent.Value,
                Type = EventType.Debit.ToString()
            });
        }
    }
}
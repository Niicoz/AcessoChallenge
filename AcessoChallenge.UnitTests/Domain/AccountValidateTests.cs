using AcessoChallenge.Domain;
using AcessoChallenge.Domain.Enums;
using AcessoChallenge.Domain.Events;
using AcessoChallenge.Domain.Services;
using AcessoChallenge.Infrastructure.Contracts;
using AcessoChallenge.UnitTests.Autofixture;
using AutoFixture.Idioms;
using NSubstitute;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AcessoChallenge.UnitTests.Domain
{
    public class AccountValidateTests
    {
        [Theory, AutoNSubstituteData]
        public void UserController_Should_Guard_Its_Clause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AccountValidate).GetConstructors());
        }

        [Theory, AutoNSubstituteData]
        public async Task Validate_Should_ChangeStatus_To_Processing_And_Publish_When_Accounts_Is_Ok(
            TransferSolicitationEvent solicitationEvent,
            HttpResponseMessage messageOrigin,
            HttpResponseMessage messageDestination,
            Account accountOrigin,
            Account accountDestination,
            AccountValidate sut)
        {
            //Assert
            var successOrigin = ServiceResult<Account>.Success(accountOrigin);
            var successDestination = ServiceResult<Account>.Success(accountDestination);

            sut.AcessoApiClient.CheckAccount(solicitationEvent.AccountOrigin).Returns(messageOrigin);
            sut.AcessoApiClient.CheckAccount(solicitationEvent.AccountDestination).Returns(messageDestination);

            sut.ResponseValidator.ResponseValidate(messageOrigin).Returns(successOrigin);
            sut.ResponseValidator.ResponseValidate(messageDestination).Returns(successDestination);

            //Act
            await sut.Validate(solicitationEvent);

            //Assert
            await sut.FundTransferRepository.Received().UpdateFundTransferStatusAsync(
                solicitationEvent.TransactionId, Status.Processing);

            await sut.Publisher.Received().Publish<TransferEvent>(Arg.Any<object>());
        }
    }
}
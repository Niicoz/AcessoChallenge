using AcessoChallenge.Api.Controllers;
using AcessoChallenge.Api.Models;
using AcessoChallenge.Domain.Entities;
using AcessoChallenge.Domain.Events;
using AcessoChallenge.UnitTests.Autofixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AcessoChallenge.UnitTests.Api.Controllers
{
    public class FundTransferControllerTests
    {
        [Theory, AutoNSubstituteData]
        public void UserController_Should_Guard_Its_Clause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(FundTransferController).GetConstructors());
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateFundTransfer_Should_Persist_And_Call_Publisher(
            FundTransferRequest request,
            FundTransferController sut)
        {
            //Assert

            //Act
            var actual = await sut.CreateFundTransfer(request);

            //Assert
            await sut.FundTransferRepository.Received().InsertFundTransferAsync(request.TransactionId);

            await sut.Publisher.Received().Publish<TransferSolicitationEvent>(Arg.Any<object>());

            sut.Logger.LogInformation("Transaction created {transactionId}", request.TransactionId);

            actual.As<CreatedResult>().Value.Should().BeEquivalentTo(new { request.TransactionId });
        }

        [Theory, AutoNSubstituteData]
        public async Task GetFundTransferStatus_Should_Return_Status_When_Id_Exists(
            Guid transactionId,
            FundTransferStatus fundTransferStatus,
            FundTransferController sut)
        {
            //Assert
            sut.FundTransferRepository.GetStatusByIdAsync(transactionId).Returns(fundTransferStatus);

            //Act
            var actual = await sut.GetFundTransferStatus(transactionId);

            //Assert
            await sut.FundTransferRepository.Received().GetStatusByIdAsync(transactionId);

            actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(fundTransferStatus);
        }

        [Theory, AutoNSubstituteData]
        public async Task GetFundTransferStatus_Should_Return_NotFound_When_Id_Dont_Exists(
            Guid transactionId,
            FundTransferController sut)
        {
            //Assert
            FundTransferStatus fundTransferStatus = null;
            sut.FundTransferRepository.GetStatusByIdAsync(transactionId).Returns(fundTransferStatus);

            //Act
            var actual = await sut.GetFundTransferStatus(transactionId);

            //Assert
            await sut.FundTransferRepository.Received().GetStatusByIdAsync(transactionId);

            actual.Should().BeOfType<NotFoundResult>();
        }
    }
}
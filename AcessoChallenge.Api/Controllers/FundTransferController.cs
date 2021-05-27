using AcessoChallenge.Api.Models;
using AcessoChallenge.Domain.Events;
using AcessoChallenge.Domain.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AcessoChallenge.Api.Controllers
{
    public class FundTransferController : ControllerBase
    {
        public IFundTransferRepository FundTransferRepository { get; }
        public IPublishEndpoint Publisher { get; }
        public ILogger<FundTransferController> Logger { get; }

        public FundTransferController(
            IFundTransferRepository fundTransferRepository,
            IPublishEndpoint publisher,
            ILogger<FundTransferController> logger)
        {
            FundTransferRepository = fundTransferRepository ?? throw new ArgumentNullException(nameof(fundTransferRepository));
            Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("fund-transfer")]
        public async Task<IActionResult> CreateFundTransfer([FromBody] FundTransferRequest request)
        {
            await FundTransferRepository.InsertFundTransferAsync(request.TransactionId);

            await Publisher.Publish<TransferSolicitationEvent>(new
            {
                request.TransactionId,
                request.AccountOrigin,
                request.AccountDestination,
                request.Value
            });

            Logger.LogInformation("Transaction created {transactionId}", request.TransactionId);

            return Created("", new { request.TransactionId });
        }

        [HttpGet("fund-transfer/{transactionId}")]
        public async Task<IActionResult> GetFundTransferStatus(Guid transactionId)
        {
            var transferStatus = await FundTransferRepository.GetStatusByIdAsync(transactionId);

            if (transferStatus == null)
                return NotFound();

            return Ok(transferStatus);
        }
    }
}
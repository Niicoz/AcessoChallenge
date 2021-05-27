using AcessoChallenge.Api.Models;
using FluentValidation;

namespace AcessoChallenge.Api.ModelValidators
{
    public class FundTransferRequestValidator : AbstractValidator<FundTransferRequest>
    {
        public FundTransferRequestValidator()
        {
            RuleFor(o => o.AccountOrigin)
                .NotEmpty()
                .WithMessage("AccountOrigin is Required");

            RuleFor(o => o.AccountDestination)
                .NotEmpty()
                .WithMessage("AccountDestination is Required");

            RuleFor(o => o.Value)
                .GreaterThan(0)
                .WithMessage("Must be greater than 0");
        }
    }
}
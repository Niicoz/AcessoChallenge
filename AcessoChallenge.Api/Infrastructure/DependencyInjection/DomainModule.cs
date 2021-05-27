using AcessoChallenge.Domain.Interfaces;
using AcessoChallenge.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AcessoChallenge.Api.Infrastructure.DependencyInjection
{
    public static class DomainModule
    {
        public static void AddDomainModule(this IServiceCollection services)
        {
            services.AddTransient<ITransferProcess, TransferProcess>();

            services.AddTransient<IAccountValidate, AccountValidate>();
        }
    }
}
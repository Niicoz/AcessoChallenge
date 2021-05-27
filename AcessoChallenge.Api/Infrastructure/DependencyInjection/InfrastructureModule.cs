using AcessoChallenge.Domain.Interfaces;
using AcessoChallenge.Infrastructure.Clients;
using AcessoChallenge.Infrastructure.Factories;
using AcessoChallenge.Infrastructure.Repositories;
using AcessoChallenge.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace AcessoChallenge.Api.Infrastructure.DependencyInjection
{
    public static class InfrastructureModule
    {
        public static void AddInfrastructureModule(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddSingleton<IHttpClientWrapper>(resolver =>
            {
                var httpClientFactory = resolver.GetService<IHttpClientFactory>();

                return new HttpClientWrapper(httpClientFactory.CreateClient());
            });

            services.AddSingleton<IDbConnectionFactory>(resolver =>
            {
                var configuration = resolver.GetService<IConfiguration>();

                var splitDbConnectionString = configuration.GetConnectionString("MSSQL");

                return new SimpleDbConnectionFactory(splitDbConnectionString);
            });

            services.AddTransient<IAcessoApiClient>(resolver =>
            {
                var configuration = resolver.GetService<IConfiguration>();

                var httpClient = resolver.GetService<IHttpClientWrapper>();

                var acessoUrl = configuration.GetValue<string>("AcessoApiUrl");

                return new AcessoApiClient(httpClient, acessoUrl);
            });

            services.AddTransient<IFundTransferRepository, FundTransferRepository>();

            services.AddTransient<IResponseValidator, ResponseValidator>();
        }
    }
}
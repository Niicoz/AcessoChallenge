using AcessoChallenge.Infrastructure.Consumers;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AcessoChallenge.Api.Infrastructure.DependencyInjection
{
    public static class MassTransit
    {
        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitConnectionString = configuration.GetConnectionString("RabbitMq");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<FundTransferValidateConsumer>();

                x.AddConsumer<TransferEventConsumer>();

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq");

                    cfg.ConfigureEndpoints(context);

                    cfg.UseMessageRetry(r =>
                    {
                        r.Exponential(10, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(5));
                        r.Handle<Exception>();
                    });
                });
            });
        }
    }
}
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace AcessoChallenge.Api.Infrastructure.DependencyInjection
{
    public static class ApiModule
    {
        public static void AddApiModule(this IServiceCollection services)
        {
            services.AddSingleton<ValidationFilter>();

            services
                .AddControllers(opt =>
                {
                    opt.Filters.Add<ValidationFilter>();
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;

                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }
    }
}
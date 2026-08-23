using FlowersApp.Shared.Models;
using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowersApp.Shared.Extensions;

public static class RegisterRabbitMQ
{
    public static IServiceCollection RegisterRabbitMq<TAssemblyMarker>(this IServiceCollection services ,IConfiguration configuration)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumers(typeof(TAssemblyMarker).Assembly);
            cfg.UsingRabbitMq((context, cfg) =>
            {
                // 2. Configure RabbitMQ connection
                var rabbitMqSettings = configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
                var rabbitMqHost = rabbitMqSettings.Host ?? "localhost";
                var rabbitMqUser = rabbitMqSettings.UserName ?? "guest";
                var rabbitMqPass = rabbitMqSettings.Password ?? "guest";

                cfg.Host(rabbitMqHost, "/", hostConfigurator => {
                    hostConfigurator.Username(rabbitMqUser);
                    hostConfigurator.Password(rabbitMqPass);
                });

                // 3. Configure retry policy for resilience
                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                // 4. Configure endpoints (this will auto-configure queues for your consumers)
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}

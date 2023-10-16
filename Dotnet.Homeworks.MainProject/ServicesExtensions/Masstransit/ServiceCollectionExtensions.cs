using Dotnet.Homeworks.Shared.RabbitMq;
using MassTransit;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services,
        RabbitMqConfig rabbitConfiguration)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.UsingRabbitMq((context, config) =>
            {
                config.Host(rabbitConfiguration.FullHostname);
                config.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}
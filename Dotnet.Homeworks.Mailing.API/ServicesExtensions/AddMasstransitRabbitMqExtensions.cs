using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Consumers;
using Dotnet.Homeworks.Shared.RabbitMq;
using MassTransit;

namespace Dotnet.Homeworks.Mailing.API.ServicesExtensions;

public static class AddMasstransitRabbitMqExtensions
{
    public static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services,
        RabbitMqConfig rabbitConfiguration)
    {
        services.AddScoped<IEmailConsumer, EmailConsumer>();
        
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<IEmailConsumer>();
            
            cfg.UsingRabbitMq((context, config) =>
            {
                config.Host(rabbitConfiguration.FullHostname);
                config.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailConfig>(configuration.GetSection("EmailConfig"));
        services.Configure<RabbitMqConfig>(configuration.GetSection("RabbitMqConfig"));
        return services;
    }
}
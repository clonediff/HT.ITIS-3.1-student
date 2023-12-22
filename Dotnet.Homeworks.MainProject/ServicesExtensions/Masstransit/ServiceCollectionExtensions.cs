using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Behaviors;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using Dotnet.Homeworks.Shared.RabbitMq;
using FluentValidation;
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

    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfig>(configuration.GetSection("RabbitMqConfig"));
        return services;
    }

    public static IServiceCollection AddCQRS(this IServiceCollection services)
    {
        services
            .AddTransient<IProductRepository, ProductRepository>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddTransient<IUnitOfWork, UnitOfWork>()
            .AddMediator(AssemblyReference.Assembly)
            .AddValidatorsFromAssembly(AssemblyReference.Assembly, ServiceLifetime.Transient)
            .AddPermissionChecks(AssemblyReference.Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PermissionCheckBehavior<,>));
        
        return services;
    }
}
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    private static readonly Type RequestHandlerWithResponseType = typeof(IRequestHandler<,>);
    private static readonly Type RequestHandlerType = typeof(IRequestHandler<>);
    
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] handlersAssemblies)
    {
        var allHandlers = handlersAssemblies.SelectMany(x => x.GetTypes().Where(y =>
            y.GetInterfaces().Any(z =>
                z.IsGenericType && (z.GetGenericTypeDefinition() == RequestHandlerWithResponseType ||
                                    z.GetGenericTypeDefinition() == RequestHandlerType))));

        foreach (var handler in allHandlers)
        {
            var requestHandlerInt = handler.GetInterfaces().First(x =>
                x.IsGenericType && (x.GetGenericTypeDefinition() == RequestHandlerWithResponseType ||
                                    x.GetGenericTypeDefinition() == RequestHandlerType));
            services.AddTransient(requestHandlerInt, handler);
        }

        services.AddTransient<IMediator, Mediator>();
        
        return services;
    }
}
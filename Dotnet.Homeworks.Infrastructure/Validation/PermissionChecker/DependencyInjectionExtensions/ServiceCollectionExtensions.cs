using System.Reflection;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    private static readonly Type PermissionCheckType = typeof(IPermissionCheck<>);
    
    public static IServiceCollection AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly assembly
    )
    {
        serviceCollection.AddPermissionChecks(new[] { assembly });
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly[] assemblies
    )
    {
        var implemetations = assemblies
            .SelectMany(x => x.GetTypes().Where(y =>
                y.GetInterfaces().Any(z =>
                    z.IsGenericType && z.GetGenericTypeDefinition() == PermissionCheckType)))
            .Select(x => new
            {
                implType = x,
                intType = x.GetInterfaces().First(y => y.IsGenericType && y.GetGenericTypeDefinition() == PermissionCheckType),
            });
        foreach (var implemetation in implemetations)
            serviceCollection.AddTransient(implemetation.intType, implemetation.implType);

        return serviceCollection;
    }
}
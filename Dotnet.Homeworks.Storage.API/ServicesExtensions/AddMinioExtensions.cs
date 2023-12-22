using Minio;
using MinioConfig = Dotnet.Homeworks.Storage.API.Configuration.MinioConfig;

namespace Dotnet.Homeworks.Storage.API.ServicesExtensions;

public static class AddMinioExtensions
{
    public static IServiceCollection AddMinioClient(this IServiceCollection services,
        MinioConfig minioConfiguration)
    {
        services.AddMinio(configuration =>
        {
            configuration.WithSSL(minioConfiguration.WithSsl);
            configuration.WithEndpoint(minioConfiguration.Endpoint, minioConfiguration.Port);
            configuration.WithCredentials(minioConfiguration.Username, minioConfiguration.Password);
        });

        return services;
    }
}
using Dotnet.Homeworks.Storage.API.Configuration;
using Dotnet.Homeworks.Storage.API.Endpoints;
using Dotnet.Homeworks.Storage.API.Services;
using Dotnet.Homeworks.Storage.API.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

var minioConfig = builder.Configuration.GetSection("MinioConfig").Get<MinioConfig>()!;
builder.Services.Configure<MinioConfig>(builder.Configuration.GetSection("MinioConfig"))
    .AddMinioClient(minioConfig)
    .AddSingleton<IStorageFactory, StorageFactory>()
    .AddHostedService<PendingObjectsProcessor>();

var app = builder.Build();

app.MapProductsEndpoints();

app.Run();
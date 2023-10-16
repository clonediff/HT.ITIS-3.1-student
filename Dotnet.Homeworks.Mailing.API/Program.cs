using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Consumers;
using Dotnet.Homeworks.Mailing.API.Services;
using Dotnet.Homeworks.Mailing.API.ServicesExtensions;
using Dotnet.Homeworks.Shared.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

var rabbitMqConfig = builder.Configuration.GetSection("RabbitMqConfig").Get<RabbitMqConfig>()!;
builder.Services.AddMasstransitRabbitMq(rabbitMqConfig)
    .ConfigureOptions(builder.Configuration);

builder.Services.AddScoped<IMailingService, MailingService>();

var app = builder.Build();

app.Run();
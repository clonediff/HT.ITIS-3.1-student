using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Consumers;
using Dotnet.Homeworks.Mailing.API.Services;
using Dotnet.Homeworks.Mailing.API.ServicesExtensions;
using Dotnet.Homeworks.Shared.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMqConfig"));

builder.Services.AddScoped<IEmailConsumer, EmailConsumer>();
var rabbitMqConfig = builder.Configuration.GetSection("RabbitMqConfig").Get<RabbitMqConfig>()!;
builder.Services.AddMasstransitRabbitMq(rabbitMqConfig);

builder.Services.AddScoped<IMailingService, MailingService>();

var app = builder.Build();

app.Run();
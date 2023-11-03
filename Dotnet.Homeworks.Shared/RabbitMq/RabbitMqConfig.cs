namespace Dotnet.Homeworks.Shared.RabbitMq;

public class RabbitMqConfig
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Hostname { get; set; }
    public int Port { get; set; }
    
    public string FullHostname => $"amqp://{Username}:{Password}@{Hostname}:{Port}";
}

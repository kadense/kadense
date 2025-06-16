using Kadense.Malleable.Workflow.Connections;

namespace Kadense.Malleable.Workflow.Queuing.RabbitMQ;

public class RabbitMQConnectionOptions : MalleableWorkflowConnectionOptions
{
    public RabbitMQConnectionOptions(string? exchangeKey = null, string? queueName = null, string? routingKey = null, string? connectionString = null, string? hostName = null, int? port = null, string? exchangeType = null)
    {
        ExchangeKey = exchangeKey ?? Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGE_KEY")!;
        RoutingKey = routingKey ?? Environment.GetEnvironmentVariable("RABBITMQ_ROUTING_KEY")!;
        ExchangeType = exchangeType ?? Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGE_TYPE") ?? "fanout";
        HostName = hostName ?? Environment.GetEnvironmentVariable("RABBITMQ_HOST_NAME") ?? "localhost";
        if (port.HasValue)
        {
            Port = port.Value;
        }
        else if (Environment.GetEnvironmentVariable("RABBITMQ_PORT") is string portString && int.TryParse(portString, out var parsedPort))
        {
            Port = parsedPort;
        }

    }

    public string? ExchangeKey { get; }
    public string? RoutingKey { get; }
    public string HostName { get; }
    public int Port { get; } = 5672;
    public string ExchangeType { get; }
}

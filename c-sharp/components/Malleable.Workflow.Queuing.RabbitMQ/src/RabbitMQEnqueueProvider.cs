using System.Text.Json;
using Kadense.Malleable.Workflow.Queuing;
using Kadense.Models.Malleable;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Kadense.Malleable.Workflow.Queuing.RabbitMQ;

public class RabbitMQEnqueueProvider : EnqueueProvider
{
    public RabbitMQEnqueueProvider(RabbitMQEnqueueProviderOptions options) : base(options)
    {
        Contexts = new Dictionary<string, RabbitMQContext>();
    }

    public Dictionary<string, RabbitMQContext> Contexts { get; }

    public new RabbitMQEnqueueProviderOptions Options => (RabbitMQEnqueueProviderOptions)base.Options;

    public override async Task ConfigureStepAsync(MalleableWorkflowContext context, string stepName, string qualifiedName)
    {
        var step = context.Workflow.Spec!.Steps![stepName];

        step.Options!.Parameters.TryGetValue("queueName", out var queueName);
        queueName = queueName ?? Options.QueueName ?? qualifiedName;

        step.Options!.Parameters.TryGetValue("exchangeKey", out var exchangeKey);
        exchangeKey = exchangeKey ?? Options.ExchangeKey ?? string.Empty;

        step.Options!.Parameters.TryGetValue("routingKey", out var routingKey);
        routingKey = routingKey ?? Options.RoutingKey ?? qualifiedName;

        step.Options!.Parameters.TryGetValue("hostName", out var hostName);
        hostName = hostName?? Options.HostName;

        step.Options!.Parameters.TryGetValue("exchangeType", out var exchangeType);
        exchangeType = exchangeType ?? Options.ExchangeType;

        step.Options!.Parameters.TryGetValue("port", out var port);
        var portValue = port != null ? int.Parse(port) : Options.Port;

        var connectionFactory = new ConnectionFactory
        {
            HostName = hostName,
            Port = portValue
        };

        var connection = await connectionFactory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queueName, true, false, false, null);
        if (!string.IsNullOrEmpty(exchangeKey))
        {
            await channel.ExchangeDeclareAsync(exchangeKey, exchangeType);
        }
        var rmqContext = new RabbitMQContext(context, channel, queueName, routingKey, exchangeKey);
        Contexts[qualifiedName] = rmqContext;    
    }

    public override async Task EnqueueAsync<T>(string qualifiedName, T message)
    {
        var context = Contexts[qualifiedName];
        
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, message, context.WorkflowContext.GetJsonSerializerOptions());
            stream.Position = 0;
            var byteArray = new byte[stream.Length];
            await stream.ReadAsync(byteArray, 0, (int)stream.Length);
            var body = new ReadOnlyMemory<byte>(byteArray);
            await context.Channel.BasicPublishAsync(exchange: context.ExchangeKey, routingKey: context.RoutingKey, body: body);
        }
    }
}
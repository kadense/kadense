using System.Text.Json;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Kadense.Models.Malleable;

namespace Kadense.Malleable.Workflow.Queuing.RabbitMQ;

public class RabbitMQConnection : MalleableWorkflowConnection<RabbitMQConnectionOptions>, IListeningWorkflowConnection
{
    public RabbitMQConnection(MalleableWorkflowContext workflowContext, RabbitMQConnectionOptions options) : base(workflowContext, options)
    {
    }

    public RabbitMQContext? Context { get; protected set; }

    public void Listen<TMessage>()
        where TMessage : MalleableBase
    {
        Context!.Channel.QueueBindAsync(queue: Context.QueueName, exchange: Context.ExchangeKey, routingKey: Context.RoutingKey)
            .Wait();
        var consumer = new AsyncEventingBasicConsumer(Context.Channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            using var stream = new MemoryStream(body);
            var envelope = await JsonSerializer.DeserializeAsync<MalleableEnvelope<TMessage>>(stream, this.GetJsonSerializerOptions());
            if (envelope != null)
            {
                var task = (Task)OnReceive.DynamicInvoke(envelope)!;
                await task;
            }
        };
        Context.Channel.BasicConsumeAsync(queue: Context.QueueName, autoAck: true, consumer: consumer);
    }

    public Delegate OnReceive { get; set; } = new Func<MalleableBase, Task>(message =>
    {
        throw new NotImplementedException("OnReceive is not implemented");
    });


    public override void Send<TMessage>(TMessage message)
    {
        SendAsync(message)
            .Wait();
    }

    public async Task SendAsync<TMessage>(TMessage message)
    {
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, message, this.GetJsonSerializerOptions());
            stream.Position = 0;
            var byteArray = new byte[stream.Length];
            await stream.ReadAsync(byteArray, 0, (int)stream.Length);
            var body = new ReadOnlyMemory<byte>(byteArray);
            await Context!.Channel.BasicPublishAsync(exchange: Context.ExchangeKey, routingKey: Context.RoutingKey, body: body);
        }
    }

    public override void Initialize(MalleableWorkflowContext workflowContext, string stepName)
    {
        var step = workflowContext.Workflow.Spec!.Steps![stepName];
        var queueName = $"{workflowContext.Workflow.Metadata.NamespaceProperty}.{workflowContext.Workflow.Metadata.Name}.{stepName}";
        var connectionFactory = new ConnectionFactory
        {
            HostName = Options.HostName,
            Port = Options.Port
        };

        Task.Run(async () =>
        {
            var connection = await connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            Context = new RabbitMQContext(workflowContext, channel, queueName, Options.RoutingKey!, Options.ExchangeKey!);

            await Context!.Channel.ExchangeDeclareAsync(exchange: Context.ExchangeKey, type: ExchangeType.Fanout);
            await Context!.Channel.QueueDeclareAsync(queue: Context.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }).Wait();
    }
}

using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Channels;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Connections;
using Kadense.Malleable.Workflow.Processing;
using RabbitMQ.Client;

namespace Kadense.Malleable.Workflow.RabbitMQ
{
        public class RMQWriteProcessor<TIn, TOut> : MalleableWorkflowProcessor<TIn, TOut>
        where TIn : MalleableBase
        where TOut : MalleableBase
    {
        public RMQWriteProcessor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
        {
            var qualifiedName = $"{context.Workflow.Metadata.NamespaceProperty}.{context.Workflow.Metadata.Name}.{stepName}";
            StepDefinition.Options!.Parameters.TryGetValue("queueName", out var queueName);
            queueName = queueName ?? qualifiedName; 
            
            StepDefinition.Options!.Parameters.TryGetValue("exchangeKey", out var exchangeKey);
            exchangeKey = exchangeKey ?? string.Empty;
            
            StepDefinition.Options!.Parameters.TryGetValue("routingKey", out var routingKey);
            routingKey = routingKey ?? qualifiedName;

            StepDefinition.Options!.Parameters.TryGetValue("hostName", out var hostName);
            hostName = hostName ?? "localhost";

            StepDefinition.Options!.Parameters.TryGetValue("exchangeType", out var exchangeType);
            exchangeType = exchangeType ?? "fanout";

            StepDefinition.Options!.Parameters.TryGetValue("port", out var port);
            var portValue = port != null ? int.Parse(port) : 5672; 

            var connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                Port = portValue
            };

            var rmqContext = Task.Run<RMQContext>(async () => {
                var connection = await connectionFactory.CreateConnectionAsync();
                var channel = await connection.CreateChannelAsync();
                await channel.QueueDeclareAsync(queueName, true, false, false, null);
                if(!string.IsNullOrEmpty(exchangeKey)){
                    await channel.ExchangeDeclareAsync(exchangeKey, exchangeType);
                }
                return new RMQContext(channel, queueName, routingKey, exchangeKey);
            }).Result;
            RMQContext = rmqContext;
        }

        public RMQContext RMQContext { get; set; }

        public override (string?, MalleableBase) Process(MalleableBase message)
        {
            if (message is not TIn input)
            {
                throw new InvalidOperationException($"Invalid message type. Expected {typeof(TIn)}, but got {message.GetType()}.");
            }
            var destination = StepDefinition.Options!.NextStep;
            var task = SendAsync(input);
            task.Wait();
            return (destination, input);
        }

        
        public async Task SendAsync<TMessage>(TMessage message)
        {
            using(var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, message, this.Context.GetJsonSerializerOptions());
                stream.Position = 0;
                var byteArray = new byte[stream.Length];
                await stream.ReadAsync(byteArray, 0, (int)stream.Length);
                var body = new ReadOnlyMemory<byte>(byteArray);
                await RMQContext.Channel.BasicPublishAsync(exchange: RMQContext.ExchangeKey, routingKey: RMQContext.RoutingKey, body: body);
            }
        }

        public override string? GetErrorDestination()
        {
            return StepDefinition.Options!.ErrorStep;
        }
    }
}
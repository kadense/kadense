using System.Text.Json;
using System.Threading.Channels;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Kadense.Malleable.Workflow.RabbitMQ
{
    public class MalleableWorkflowRMQConnection : MalleableWorkflowConnection<RMQContext>, IListeningWorkflowConnection
    {
        public MalleableWorkflowRMQConnection(IList<MalleableAssembly> assemblies, RMQContext context) : base(assemblies, context)
        {
            Context.Channel.ExchangeDeclareAsync(exchange: context.ExchangeKey, type: ExchangeType.Fanout).Wait();
            Context.Channel.QueueDeclareAsync(queue: Context.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null).Wait();
        }

        public void Listen<TMessage>() 
            where TMessage : MalleableBase
        {
            Context.Channel.QueueBindAsync(queue: Context.QueueName, exchange: Context.ExchangeKey, routingKey: Context.RoutingKey)
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
            using(var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, message, this.GetJsonSerializerOptions());
                stream.Position = 0;
                var byteArray = new byte[stream.Length];
                await stream.ReadAsync(byteArray, 0, (int)stream.Length);
                var body = new ReadOnlyMemory<byte>(byteArray);
                await Context.Channel.BasicPublishAsync(exchange: Context.ExchangeKey, routingKey: Context.RoutingKey, body: body);
            }
        }

    }
}
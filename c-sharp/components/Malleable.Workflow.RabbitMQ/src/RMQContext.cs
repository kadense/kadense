using System.Threading.Channels;
using Kadense.Malleable.Workflow.Connections;
using RabbitMQ.Client;

namespace Kadense.Malleable.Workflow.RabbitMQ
{
    public class RMQContext 
    {
        public RMQContext(IChannel channel, string queueName, string routingKey, string exchangeKey)
        {
            Channel = channel;
            RoutingKey = routingKey;
            ExchangeKey = exchangeKey;
            QueueName = queueName;
        }

        public IChannel Channel { get; }
        public string ExchangeKey { get; }
        public string QueueName { get; }
        public string RoutingKey { get; }
    }
}
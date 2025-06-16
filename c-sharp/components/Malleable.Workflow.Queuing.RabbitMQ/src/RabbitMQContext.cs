using Kadense.Models.Malleable;
using RabbitMQ.Client;

namespace Kadense.Malleable.Workflow.Queuing.RabbitMQ;

public class RabbitMQContext 
{
    public RabbitMQContext(MalleableWorkflowContext workflowContext, IChannel channel, string queueName, string routingKey, string exchangeKey)
    {
        Channel = channel;
        RoutingKey = routingKey;
        ExchangeKey = exchangeKey;
        QueueName = queueName;
        WorkflowContext = workflowContext;
    }

    public MalleableWorkflowContext WorkflowContext { get; }

    public IChannel Channel { get; }
    public string ExchangeKey { get; }
    public string QueueName { get; }
    public string RoutingKey { get; }
}
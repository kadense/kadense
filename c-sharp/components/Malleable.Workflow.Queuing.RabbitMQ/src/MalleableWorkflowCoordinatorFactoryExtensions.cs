namespace Kadense.Malleable.Workflow.Queuing.RabbitMQ;


public static class MalleableWorkflowCoordinatorFactoryExtensions
{
    public static MalleableWorkflowCoordinatorFactory AddEnqueueProvider(this MalleableWorkflowCoordinatorFactory factory, RabbitMQEnqueueProviderOptions options)
    {
        var provider = new RabbitMQEnqueueProvider(options);
        factory.WithWorkflowProvider("DefaultEnqueueProvider", provider);
        return factory;
    }

    public static MalleableWorkflowCoordinatorFactory AddEnqueueProvider(this MalleableWorkflowCoordinatorFactory factory, string providerName, RabbitMQEnqueueProviderOptions options)
    {
        var provider = new RabbitMQEnqueueProvider(options);
        factory.WithWorkflowProvider(providerName, provider);
        return factory;
    }

    public static MalleableWorkflowCoordinatorFactory AddRabbitMQConnection(this MalleableWorkflowCoordinatorFactory factory, RabbitMQConnectionOptions options, string executorType = "RabbitMQ")
    {
        var connection = new RabbitMQConnection(factory.WorkflowContext, options);
        factory.AddExternalStepAction(executorType, connection);
        return factory;
    }
}
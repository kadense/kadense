using Kadense.Malleable.Workflow;

namespace Kadense.Malleable.Workflow.Queuing;

public class EnqueueProviderOptions : MalleableWorkflowProviderOptions
{
    public EnqueueProviderOptions(string? queueName = null, string? connectionString = null)
    {
        QueueName = queueName ?? Environment.GetEnvironmentVariable("QUEUE_NAME")!;
        ConnectionString = connectionString ?? Environment.GetEnvironmentVariable("QUEUE_CONNECTION_STRING");
    }

    public string QueueName { get; protected set; }
    public string? ConnectionString { get; }
}


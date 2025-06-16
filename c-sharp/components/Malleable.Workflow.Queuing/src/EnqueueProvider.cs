using Kadense.Malleable.Reflection;
using Kadense.Models.Malleable;

namespace Kadense.Malleable.Workflow.Queuing;

public abstract class EnqueueProvider : MalleableWorkflowProvider
{
    public EnqueueProvider(EnqueueProviderOptions options)
    {
        Options = options;
    }

    public abstract Task ConfigureStepAsync(MalleableWorkflowContext context, string stepName, string qualifiedName);

    public abstract Task EnqueueAsync<T>(string qualifiedName, T message) where T : MalleableBase;

    public EnqueueProviderOptions Options { get; }
}
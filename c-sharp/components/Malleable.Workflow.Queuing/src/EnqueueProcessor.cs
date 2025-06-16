using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow;
using Kadense.Malleable.Workflow.Processing;
using Kadense.Models.Malleable;

namespace Kadense.Malleable.Workflow.Queuing;

[MalleableWorkflowProcessor("Enqueue", typeof(EnqueueProcessor<,>))]
public class EnqueueProcessor<TIn, TOut> : MalleableWorkflowProcessor<TIn, TOut>
    where TIn : MalleableBase
    where TOut : MalleableBase
{
    public EnqueueProcessor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
    {
        var step = context.Workflow.Spec!.Steps![stepName];
        var qualifiedName = $"{context.Workflow.Metadata.NamespaceProperty}.{context.Workflow.Metadata.Name}.{stepName}";
        QualifiedName = qualifiedName;
        if (!step.Options!.Parameters.TryGetValue("Provider", out string? provider))
            provider = "DefaultEnqueueProvider";

        Provider = (EnqueueProvider)context.Providers[provider]!;

        Task.Run(() => Provider.ConfigureStepAsync(context, stepName, QualifiedName)).Wait();
    }

    public string QualifiedName { get; }

    public EnqueueProvider Provider { get; }
    
    protected string GetExpressionValue(Delegate expression, TIn message)
    {
        var args = new object[]{
            message
        };
        return (string)expression.DynamicInvoke(args)!;
    }

    public override string? GetErrorDestination()
    {
        return StepDefinition.Options!.ErrorStep;
    }

    public override (string?, MalleableBase) Process(MalleableBase message)
    {
        var destination = StepDefinition.Options!.NextStep;
        var task = Provider.EnqueueAsync(QualifiedName, message);
        task.Wait();
        return (destination, message);
    }
}
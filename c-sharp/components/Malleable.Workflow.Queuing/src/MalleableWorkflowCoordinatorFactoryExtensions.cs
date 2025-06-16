namespace Kadense.Malleable.Workflow.Queuing;

public static class MalleableWorkflowCoordinatorFactoryExtensions
{
    public static MalleableWorkflowCoordinatorFactory AddEnqueueAction(this MalleableWorkflowCoordinatorFactory factory, string name)
    {
        factory.AddAction(name, typeof(EnqueueProcessor<,>));
        return factory;
    }
    public static MalleableWorkflowCoordinatorFactory AddEnqueueAction(this MalleableWorkflowCoordinatorFactory factory)
    {
        factory.AddAction(typeof(EnqueueProcessor<,>));
        return factory;
    }
}

namespace Kadense.Malleable.Workflow.Email;

public static class MalleableWorkflowCoordinatorFactoryExtensions
{
    public static MalleableWorkflowCoordinatorFactory AddSendEmail(this MalleableWorkflowCoordinatorFactory factory, string actionType)
    {
        factory.AddAction(actionType, typeof(SendEmailProcessor<,>));
        return factory;
    }
    public static MalleableWorkflowCoordinatorFactory AddSendEmail(this MalleableWorkflowCoordinatorFactory factory)
    {
        factory.AddAction(typeof(SendEmailProcessor<,>));
        return factory;
    }
}

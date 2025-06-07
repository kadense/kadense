namespace Kadense.Malleable.Workflow.Email.AmazonSES;
public static class MalleableWorkflowCoordinatorFactoryExtensions
{
    public static void AddSendEmailProvider(this MalleableWorkflowCoordinatorFactory factory, AmazonSESSendEmailProviderOptions options)
    {
        var provider = new AmazonSESSendEmailProvider(options);
        factory.WithWorkflowProvider("DefaultSendEmailProvider", provider);
    }

    public static void AddSendEmailProvider(this MalleableWorkflowCoordinatorFactory factory, string providerName, AmazonSESSendEmailProviderOptions options)
    {
        var provider = new AmazonSESSendEmailProvider(options);
        factory.WithWorkflowProvider(providerName, provider);
    }
}
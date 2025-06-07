namespace Kadense.Malleable.Workflow.Email.AzureECS;

public static class MalleableWorkflowCoordinatorFactoryExtensions
{
    public static void AddSendEmailProvider(this MalleableWorkflowCoordinatorFactory factory, AzureECSSendEmailProviderOptions options)
    {
        var provider = new AzureECSSendEmailProvider(options);
        factory.WithWorkflowProvider("DefaultSendEmailProvider", provider);
    }

    public static void AddSendEmailProvider(this MalleableWorkflowCoordinatorFactory factory, string providerName, AzureECSSendEmailProviderOptions options)
    {
        var provider = new AzureECSSendEmailProvider(options);
        factory.WithWorkflowProvider(providerName, provider);
    }
}

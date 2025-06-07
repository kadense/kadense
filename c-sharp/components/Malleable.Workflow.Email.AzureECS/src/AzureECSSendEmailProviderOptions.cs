using Kadense.Malleable.Workflow.Email;

namespace Kadense.Malleable.Workflow.Email.AzureECS
{
    public class AzureECSSendEmailProviderOptions : SendEmailProviderOptions
    {
        public AzureECSSendEmailProviderOptions(string? connectionStringName, string? sender = null, string? recipient = null, string? subject = null, string? body = null, string? bodyPlainText = null)
            : base(sender, recipient, subject, body)
        {
            AzureEcsConnectionString = Environment.GetEnvironmentVariable(connectionStringName ?? "AZURE_ECS_CONNECTION_STRING")
                                       ?? throw new InvalidOperationException($"Environment variable '{connectionStringName}' is not set.");
        }

        internal string? AzureEcsConnectionString { get; set; }
    }
}
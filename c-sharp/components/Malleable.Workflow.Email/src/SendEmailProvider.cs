using Kadense.Malleable.Reflection;
using Kadense.Models.Malleable;

namespace Kadense.Malleable.Workflow.Email
{
    public abstract class SendEmailProvider : MalleableWorkflowProvider
    {
        public SendEmailProvider(SendEmailProviderOptions options)
        {
            Options = options;
        }

        public abstract Task SendEmailAsync(string sender, string recipient, string subject, string bodyHtml, string bodyPlainText);
        
        public SendEmailProviderOptions Options { get; }
    }
}
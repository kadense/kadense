using Kadense.Malleable.Workflow.Email;
using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Logging;
using Kadense.Logging;

namespace Kadense.Malleable.Workflow.Email.AzureECS
{
    public class AzureECSSendEmailProvider : SendEmailProvider
    {
        private KadenseLogger<AzureECSSendEmailProvider> logger { get; set; }
        public AzureECSSendEmailProvider(AzureECSSendEmailProviderOptions options)
            : base(options)
        {
            logger = new KadenseLogger<AzureECSSendEmailProvider>();
            Client = new EmailClient(options.ConnectionString);
        }
        
        public EmailClient Client { get; }

        public override async Task SendEmailAsync(string sender, string recipient, string subject, string bodyHtml, string bodyPlainText)
        {
            var emailContent = new EmailContent(subject)
            {
                PlainText = bodyPlainText,
                Html = bodyHtml
            };

            var emailMessage = new EmailMessage(sender, recipient, emailContent);

            var emailSendOperation = await Client.SendAsync(
                wait: WaitUntil.Completed,
                message: emailMessage
                );
            
            logger.LogInformation($"Email sent successfully with ID: {emailSendOperation.Id}");
        }
    }
}
namespace Kadense.Malleable.Workflow.Email
{
    public class SendEmailProviderOptions : MalleableWorkflowProviderOptions
    {
        public SendEmailProviderOptions(string? sender = null, string? recipient = null, string? subject = null, string? bodyHtml = null, string? bodyPlainText = null)
        {
            Sender = sender ?? Environment.GetEnvironmentVariable("EMAIL_SENDER")!;
            Recipient = recipient ?? Environment.GetEnvironmentVariable("EMAIL_RECIPIENT");
            Subject = subject ?? Environment.GetEnvironmentVariable("EMAIL_SUBJECT");
            BodyHtml = bodyHtml ?? Environment.GetEnvironmentVariable("EMAIL_BODY_HTML");
            BodyPlainText = bodyPlainText ?? Environment.GetEnvironmentVariable("EMAIL_BODY_PLAIN_TEXT");
        }

        public string Sender { get; }
        public string? Recipient { get; }
        public string? Subject { get; }
        public string? BodyHtml { get; }
        public string? BodyPlainText { get; }
    }
}
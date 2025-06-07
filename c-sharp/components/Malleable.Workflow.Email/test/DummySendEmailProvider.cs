
namespace Kadense.Malleable.Workflow.Email.Tests {
    public class DummySendEmailProviderOptions : SendEmailProviderOptions
    {
        public DummySendEmailProviderOptions(Action<string, string, string, string, string> onSendEmail, string? sender = null, string? recipient = null, string? subject = null, string? bodyHtml = null, string? bodyPlainText = null)
            : base(sender, recipient, subject, bodyHtml, bodyPlainText)
        {
            OnSendEmail = onSendEmail;
        }

        private Action<string, string, string, string, string> OnSendEmail;

        public Action<string, string, string, string, string> GetAction()
        {
            return OnSendEmail;
        }
    }
    public class DummySendEmailProvider : SendEmailProvider
    {
        public DummySendEmailProvider(DummySendEmailProviderOptions options) : base(options)
        {

        }
        public override Task SendEmailAsync(string sender, string recipient, string subject, string bodyHtml, string bodyPlainText)
        {
            DummySendEmailProviderOptions opts = (DummySendEmailProviderOptions)Options;
            var onSendEmail = opts.GetAction();
            onSendEmail(sender, recipient, subject, bodyHtml, bodyPlainText);
            return Task.CompletedTask;
        }
    }
}
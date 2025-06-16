using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Processing;
using Kadense.Models.Malleable;
using System.Text.RegularExpressions;

namespace Kadense.Malleable.Workflow.Email
{
    [MalleableWorkflowProcessor("SendEmail", typeof(SendEmailProcessor<,>))]
    public class SendEmailProcessor<TIn, TOut> : MalleableWorkflowProcessor<TIn, TOut>
        where TIn : MalleableBase
        where TOut : MalleableBase
    {
        public SendEmailProcessor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
        {
            var step = context.Workflow.Spec!.Steps![stepName];

            if (!step.Options!.Parameters.TryGetValue("Provider", out string? provider))
                provider = "DefaultSendEmailProvider";

            Provider = (SendEmailProvider)context.Providers[provider]!;


            if (!StepDefinition.Options!.Parameters.TryGetValue("Sender", out string? sender))
                sender = Provider.Options.Sender;

            if (!StepDefinition.Options!.Parameters.TryGetValue("Recipient", out string? recipient))
                recipient = Provider.Options.Recipient;

            if (!StepDefinition.Options!.Parameters.TryGetValue("Subject", out string? subject))
                subject = Provider.Options.Subject ?? "Email from {StepName} on Malleable Workflow {Workflow.Metadata.Name} on {Workflow.Metadata.NamespaceProperty}";

            if (!StepDefinition.Options!.Parameters.TryGetValue("BodyHtml", out string? bodyHtml))
                bodyHtml = Provider.Options.BodyHtml;

            if (!StepDefinition.Options!.Parameters.TryGetValue("BodyPlainText", out string? bodyPlainText))
                bodyPlainText = Provider.Options.BodyPlainText;

            if (sender == null)
                throw new InvalidOperationException("Sender is required for SendEmailProcessor.");

            if (recipient == null)
                throw new InvalidOperationException("Recipient is required for SendEmailProcessor.");

            if (subject == null)
                throw new InvalidOperationException("Subject is required for SendEmailProcessor.");

            if (bodyHtml == null)
                throw new InvalidOperationException("BodyHtml is required for SendEmailProcessor.");

            if (bodyPlainText == null)
                throw new InvalidOperationException("BodyPlainText is required for SendEmailProcessor.");

            RecipientExpression = CompileExpression<string>(recipient);
            SenderExpression = CompileExpression<string>(sender);
            SubjectExpression = CompileExpression<string>(subject);
            BodyHtmlExpression = CompileExpression<string>(bodyHtml);
            BodyPlainTextExpression = CompileExpression<string>(bodyPlainText);
        }

        

        public Delegate RecipientExpression { get; set; }
        public Delegate SenderExpression { get; set; }
        public Delegate SubjectExpression { get; set; }
        public Delegate BodyHtmlExpression { get; set; }
        public Delegate BodyPlainTextExpression { get; set; }
        public SendEmailProvider Provider { get; }
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
            var sender = GetExpressionValue(SenderExpression, (TIn)message);
            var recipient = GetExpressionValue(RecipientExpression, (TIn)message);
            var subject = GetExpressionValue(SubjectExpression, (TIn)message);
            var bodyHtml = GetExpressionValue(BodyHtmlExpression, (TIn)message);
            var bodyPlainText = GetExpressionValue(BodyPlainTextExpression, (TIn)message);
            var task = Provider.SendEmailAsync(sender, recipient, subject, bodyHtml, bodyPlainText);
            task.Wait();
            return (destination, message);
        }
    }
}
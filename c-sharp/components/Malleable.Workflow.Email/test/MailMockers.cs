using Kadense.Models.Malleable;
using Kadense.Models.Malleable.Tests;

namespace Kadense.Malleable.Workflow.Email.Tests
{
    public class MailMockers : MalleableMockers
    {
        public override MalleableWorkflow MockWorkflow()
        {
            var workflow = base.MockWorkflow();
            workflow.Spec!.Steps!["TestStep"]!.ConverterOptions!.NextStep = "SendEmailStep";
            workflow.Spec!.Steps.Remove("WriteApi");
            workflow.Spec!.Steps!["SendEmailStep"] = new MalleableWorkflowStep
            {
                Action = "SendEmail",
                Options = new MalleableWorkflowStandardActionOptions
                {
                    Parameters = new Dictionary<string, string>
                        {
                            { "Provider", "DefaultSendEmailProvider" },
                            { "Sender", "\"enquiries@kadense.io\"" },
                            { "Recipient", "\"enquiries@kadense.io\"" },
                            { "Subject", "string.Format(\"Test Email from Malleable Workflow for {0}\", Input.TestStringV1)" },
                            { "BodyHtml", "string.Format(\"<h1>Test Email</h1><p>This is a test email from Malleable Workflow for {0}.</p>\", Input.TestStringV1)" },
                            { "BodyPlainText", "string.Format(\"This is a test email from Malleable Workflow for {0}\", Input.TestStringV1)" }
                        }
                }
            };
            return workflow;
        }
    }
}
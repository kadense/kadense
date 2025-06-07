using Kadense.Malleable.Workflow.Email;
namespace Kadense.Malleable.Workflow.Email.AmazonSES;

public class AmazonSESSendEmailProviderOptions : SendEmailProviderOptions
{
    public AmazonSESSendEmailProviderOptions(string? configSet, string? sender = null, string? recipient = null, string? subject = null, string? body = null, string? bodyPlainText = null)
        : base(sender, recipient, subject, body)
    {
        ConfigSet = Environment.GetEnvironmentVariable(configSet ?? "AWS_SES_CONFIG_SET");
        RegionEndpoint = Environment.GetEnvironmentVariable(configSet ?? "AWS_SES_REGION_ENDPOINT") ?? "EUWest1";
    }

    public string? ConfigSet { get; set; }
    public string RegionEndpoint { get; set; }
}

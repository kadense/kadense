using Kadense.Logging;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon;
namespace Kadense.Malleable.Workflow.Email.AmazonSES;

public class AmazonSESSendEmailProvider : SendEmailProvider
{
    private KadenseLogger<AmazonSESSendEmailProvider> logger { get; set; }
    public AmazonSESSendEmailProvider(AmazonSESSendEmailProviderOptions options)
        : base(options)
    {
        logger = new KadenseLogger<AmazonSESSendEmailProvider>();
        Client = new AmazonSimpleEmailServiceClient(RegionEndpoint.GetBySystemName(options.RegionEndpoint));
    }

    public AmazonSimpleEmailServiceClient Client { get; }

    public override async Task SendEmailAsync(string sender, string recipient, string subject, string bodyHtml, string bodyPlainText)
    {
        var opts = Options as AmazonSESSendEmailProviderOptions;
        var sendRequest = new SendEmailRequest
                {
                    Source = sender,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { recipient }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = bodyHtml
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = bodyPlainText
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    ConfigurationSetName = opts!.ConfigSet
                };

        var response = await Client.SendEmailAsync(sendRequest);
        logger.LogInformation($"Email sent successfully with ID: {response.MessageId}");
    }
}
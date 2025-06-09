---
title: SendEmail Action
sidebar_position: 6
---
import Tabs from '@theme/Tabs';
import TabItem from '@theme/TabItem';

Sends an email based on the contents of

This extension needs to be added to the service:

<Tabs>
    <TabItem value="azure" label="Azure" default>
        First, you will need to add the package:

        ```bash
        dotnet add package Kadense.Malleable.Workflow.Email
        dotnet add package Kadense.Malleable.Workflow.Email.AzureECS
        ```

        Then add the provider and email action to your builder:

        ```csharp
        var builder = System
            .AddWorkflow(workflow, malleableAssemblyList)
            .AddSendEmailProvider(new AzureECSSendEmailProviderOptions()) // Add the default provider for sending emails
            .AddSendEmail() // Adds the send email action
            .WithDebugMode()
            .Validate()
        ```

        **Provider Options:**

        | Name | Description | Required |
        | --- | --- | --- |
        | ConnectionString | The connection string for the Azure ECS Service, if not populated it will look for this in the environment variable **AZURE_ECS_CONNECTION_STRING** | false |
        | Sender | The email of the sender, if not populated it will look for the value in the environment variable **EMAIL_SENDER** | false |
        | Recipient | The email of the recipient, if not populated it will look for the value in the environment variable **EMAIL_RECIPIENT** | false |
        | Subject | The subject line on the email, if not populated it will look for the value in the environment variable **EMAIL_SUBJECT** | false |
        | BodyHtml | The HTML body on the email, if not populated it will look for the value in the environment variable **EMAIL_BODY_HTML** | false |
        | BodyPlainText | The plain text body on the email, if not populated it will look for the value in the environment variable **EMAIL_BODY_PLAIN_TEXT** | false |

    </TabItem>
    <TabItem value="aws" label="AWS">
        First, you will need to add the package:

        ```bash
        dotnet add package Kadense.Malleable.Workflow.Email
        dotnet add package Kadense.Malleable.Workflow.Email.AmazonSES
        ```

        Then add the provider and email action to your builder:

        ```csharp
        var builder = System
            .AddWorkflow(workflow, malleableAssemblyList)
            .AddSendEmailProvider(new AmazonSESSendEmailProviderOptions() // Add the default provider for sending emails
            {
                RegionEndpoint = "EUWest1"
            })
            .AddSendEmail() // Adds the send email action
            .WithDebugMode()
            .Validate()
        ```

        **Provider Options:**

        | Name | Description | Required |
        | --- | --- | --- |
        | RegionEndpoint | The region for the endpoint of the Amazon SES Service, if not populated it will look for this in the environment variable **AWS_SES_REGION_ENDPOINT** | false |
        | ConfigSet | The config set string for the Amazon SES Service, if not populated it will look for this in the environment variable **AWS_SES_CONFIG_SET** | false |
        | Sender | The email of the sender, if not populated it will look for the value in the environment variable **EMAIL_SENDER** | false |
        | Recipient | The email of the recipient, if not populated it will look for the value in the environment variable **EMAIL_RECIPIENT** | false |
        | Subject | The subject line on the email, if not populated it will look for the value in the environment variable **EMAIL_SUBJECT** | false |
        | BodyHtml | The HTML body on the email, if not populated it will look for the value in the environment variable **EMAIL_BODY_HTML** | false |
        | BodyPlainText | The plain text body on the email, if not populated it will look for the value in the environment variable **EMAIL_BODY_PLAIN_TEXT** | false |

    </TabItem>
</Tabs>

Once you've got the provider and action added, you can add in the steps to your workflow.

```yaml
Email:
    action: SendEmail
    nextStep: ProcessMessage
    options:
        parameters:
            sender: >
                "noreply@kadense.io"
            recipient: >
                "enquiries@kadense.io"
            subject: >
                string.Format("Receipt: {0} - Received", Input.MessageId)
            bodyHtml: >
                string.Format("<h1>Welcome {0}</h1><p>Just to let you know we've received your request {1} and are processing it now.</p>", Input.FirstName, Input.MessageId)
            bodyHtml: >
                string.Format("Welcome {0}, Just to let you know we've received your request {1} and are processing it now.", Input.FirstName, Input.MessageId)
```


## Parameters
| Name | Description | Required |
| --- | --- | --- |
| provider | The name of the email provider to use, defaults to DefaultSendEmailProvider | false | 
| sender | The email of the sender, can be supplied via the options instead | false |
| receipient | The email of the recipient, can be supplied via the options instead | false |
| subject | The subject of the email, can be supplied via the options instead | false |
| bodyHtml | The HTML body of the email, can be supplied via the options instead | false |
| bodyPlainText | The plain text body of the email, can be supplied via the options instead | false |

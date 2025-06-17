---
title: Enqueue Action
sidebar_position: 5
---
import Tabs from '@theme/Tabs';
import TabItem from '@theme/TabItem';


Writes the contents to a Queue

This extension needs to be added to the service:

<Tabs>
    <TabItem value="rabbitmq" label="RabbitMQ" default>

        ```csharp
        var builder = System
            .AddWorkflow(workflow, malleableAssemblyList)
            .AddEnqueueProvider(new RabbitMQEnqueueProviderOptions // Add the RabbitMQ Provider
            {
                HostName = "localhost",
                Port = 5672
                RoutingKey = "test-queue",
            })
            .AddEnqueueAction() // Enables the Enqueue Action
            .WithDebugMode()
            .Validate()
        ```

        Then in the workflow you can add a step that writes out to

        ```yaml
        WriteSuccess:
            action: Enqueue
            options:
                parameters:
                    queueName: test-queue
        ```


        ## Parameters
        | Name | Description | Required |
        | --- | --- | --- |
        | hostName | The URL for the server | true | 
        | port | The port number for the service | false |
        | queueName| The queue name to write to | false |
        | exchangeKey | The exchange key, if not supplied no exchange is created | false |
        | routingKey | the routing key to write to | false |
        | exchangeType | if the exchangeKey is populated, the type of exchange to create (defaults to fanout) | false |

    </TabItem>
</Tabs>
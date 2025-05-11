---
title: RabbitMQWriter Action
sidebar_position: 5
---


Writes the contents to a RabbitMQ Queue

This extension needs to be added to the service:

```c#
var builder = System
    .AddWorkflow(workflow, malleableAssemblyList)
    .AddRabbitMQWriter()
    .WithDebugMode()
    .Validate()
```

```yaml
WriteSuccess:
    action: RabbitMQWriter
    options:
        parameters:
            hostName: localhost
            port: "5672"
            queueName: test-queue
            exchangeKey: ""
            routingKey: test-queue
            exchangeType: fanout
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

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
| baseUrl | The base URL to send to | true | 
| path | An expression to create the the path |

When the API is written, the body will be the data type that is input into the step. The solution assumes that it will get a response in the same format as is sent to it, however if you wish you can specify another type:

```yaml
WriteSuccessApi:
    action: WriteApi
    options:
        parameters:
            baseUrl: http://localhost:8080/
            path: >
                "api/success"
        outputType:
            className: TestResponse
            moduleName: test-module
            moduleNamespace: test-namespace

```
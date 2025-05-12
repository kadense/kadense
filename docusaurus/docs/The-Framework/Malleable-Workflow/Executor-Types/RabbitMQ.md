---
title: RabbitMQ Executor
sidebar_position: 2
---

You can add the RabbitMQ Executor type using the following command:

```csharp
// prepare the rabbit mq connection factory
var rabbitMqFactory = new ConnectionFactory{ HostName = "localhost", Port = 5672 };

// create the connection
var rabbitMqConnection = await rabbitMqFactory.CreateConnectionAsync();             

var builder = System
.AddWorkflow(workflow, malleableAssemblyList)
.AddRabbitMQWriter()
.WithDebugMode()
.Validate() // Validates and Prepares the workflow
.AddRMQConnection(rabbitMqConnection) // Adds rabbit MQ connection
.WithExternalStepActions(); // Prepare the actions which aren't part of the actor model (e.g. RabbitMQ)
```

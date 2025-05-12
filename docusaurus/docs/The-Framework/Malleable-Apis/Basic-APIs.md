---
title: Basic APIs
sidebar_position: 1
---

Once you've got your Malleable Model, you'll of course want to use it. One way in which you might want to use it is to expose an API that accepts the model.

Kadense provides a framework for creating your own uniform API's with any backing service but to keep it simple we're going to start with a Basic API

This can be done by adding a **MalleableIngressApiFileServer** to the **UseEndpoints** section of your host configuration.

```csharp
.UseEndpoints(cfg =>
{
    var malleableApi = new MalleableIngressApiFileServer(basePath: basePath);
    malleableApi.Process(cfg, malleableTypes); // Add an enumerable list of malleableTypes to be processed
});
```
*The ***basePath*** identifies where on the local file system it should save the files. When combined with mounts in Kubernetes, this can be a shared data source with multiple services.*


It will create the following endpoint:

```text
# Creates an instance of the item
POST /api/namespaces/{basePath}/{moduleNamespace}/{moduleName}/{className}
```

When called it will validate the components and write them to the following path:

```csharp
$"{basePath}/{moduleNamespace}/{moduleName}/{className}/{guid}"
```

The Guid is generated at random when the record is created.
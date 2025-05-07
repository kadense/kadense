---
title: REST APIs
sidebar_position: 2
---

In addition to a the basic API's, you can also have it create more complex API's structures, such as REST endpoints.

This can be done by adding a **MalleableRestApiFileServer** to the **UseEndpoints** section of your host configuration.

```c-sharp
.UseEndpoints(cfg =>
{
    var malleableApi = new MalleableRestApiFileServer(basePath: basePath);
    malleableApi.Process(cfg, malleableTypes); // Add an enumerable list of malleableTypes to be processed
});
```
*The ***basePath*** identifies where on the local file system it should save the files. When combined with mounts in Kubernetes, this can be a shared data source with multiple services.*

*Please also note that when using a REST API structure, the models included ***must have an identifierExpression defined***.*

It will create the following endpoints:

| Method | Path | Description |
| --- | --- | --- |
| GET | /api/namespaces/{basePath}/{moduleNamespace}/{moduleName}/{className} | Gets a list of the available items currently saved |
| GET | /api/namespaces/{basePath}/{moduleNamespace}/{moduleName}/{className}/{identifier} | Gets a unique item based upon its identifier |
| POST | /api/namespaces/{basePath}/{moduleNamespace}/{moduleName}/{className} | Creates an instance of the item |
| PUT | /api/namespaces/{basePath}/{moduleNamespace}/{moduleName}/{className}/{identifier} | Updates an existing item with a new definition |
| DELETE | /api/namespaces/{basePath}/{moduleNamespace}/{moduleName}/{className}/{identifier} | Deletes an item |

It will access the files for each of the data types in the folder:

```c-sharp
$"{basePath}/{moduleNamespace}/{moduleName}/{className}"
```

and each file will be named as follows:

```c-sharp
$"{basePath}/{moduleNamespace}/{moduleName}/{className}/{identifier}.json"
```


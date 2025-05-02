---
title: Interacting with the Custom Resources
sidebar_position: 4
---

Once your CRD is installed, you'll likely want to interact with the custom resources.

The following demonstrates how we've simplified creating generic clients to make them easier to work with.

With the default generic client that comes with the Kubernetes .Net Client, it requires you to specify the details of what you're calling every time you make a call.

The following example lists all the items in a namespace and looks for one that matches the existing resource definition. If the item already exists, it replaces it; otherwise, it creates a new one.

```csharp
var generic = new GenericClient(client, "kadense.io", "v1", "jupyternotebookinstances");

var existingItems = await genericPods.ListNamespacedAsync<KadenseCustomResourceList<JupyterNotebookInstance>>(item.Metadata.NamespaceProperty);
var filteredItems = existingItems.Items
    .Where(x => x.Metadata.Name == item.Metadata.Name)
    .ToList();

if (filteredItems.Count > 0)
{
    item.Metadata.ResourceVersion = filteredItems.First().Metadata.ResourceVersion;
    return await genericClient.ReplaceNamespacedAsync<JupyterNotebookInstance>(item, item.Metadata.NamespaceProperty);
}
else
{
    return await genericClient.CreateNamespacedAsync<JupyterNotebookInstance>(item);
}
```

As you can see, you need to specify the `apiGroup`, `apiVersion`, and `pluralName` in the constructor. Additionally, for every call, you must specify the output type. This can feel redundant since, in most cases, you'll only ever return one of two types: the list or the individual item.

Furthermore, when performing a replace operation, you need to supply the namespace as part of the parameters. However, this information is likely already populated in the item's metadata. To address these inefficiencies, we've created a wrapper class for the **GenericClient** called the **KadenseCustomResourceClient**, which can be instantiated using the **CustomResourceClientFactory**.

```csharp
var genericClientFactory = new CustomResourceClientFactory();
var genericClient = genericClientFactory.Create<JupyterNotebookInstance>(client);
var existingItems = await genericClient.ListNamespacedAsync(item.Metadata.NamespaceProperty);
var filteredItems = existingItems.Items
    .Where(x => x.Metadata.Name == item.Metadata.Name)
    .ToList();
if (filteredItems.Count > 0)
{
    item.Metadata.ResourceVersion = filteredItems.First().Metadata.ResourceVersion;
    return await genericClient.ReplaceNamespacedAsync(item);
}
else
{
    return await genericClient.CreateNamespacedAsync(item);
}
```

As you can see, this greatly simplifies working with custom resources.
---
title: Interacting with the Custom Resources
sidebar_position: 4
---

Once your CRD is installed, you'll likely want to interact with the custom resources.

The following demonstrates how we've simplified creating generic clients to make them easier to work with.

With the default generic client that comes with the Kubernetes .Net Client it requires you to add in the details of what you're calling every time you make a call.

The following example lists out all of the items in a namespace and looks for one that matches the existing resource definition, if the item already exists it replaces it, if not it will create it.

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

As you can see you need to add in the apiGroup, apiVersion and pluralName on the constructor, but every single call you need to tell function what the output of the Type is. This seems silly as in most cases you'll only ever return one of two types. The list or the individual item.

Additionally you'll note that when performing a replace you need to supply the namespace as part of the parameters, however this is likely already populated in the item metadata itself. As a result we've simplified this by creating a wrapper class to the **GenericClient** called the **KadenseCustomResourceClient** which can be created using the **CustomResourceClientFactory**.

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

As you can see this greatly simplifies working with the custom resource.
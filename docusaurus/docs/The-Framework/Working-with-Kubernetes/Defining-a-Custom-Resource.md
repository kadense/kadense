---
title: Defining a Custom Resource
sidebar_position: 2
---

The KadenseCustomResource abstract class provides all of the usual Metadata objects and so forth so you can just focus on the spec, status and scale fields for your resource. 

```csharp
[KubernetesCustomResource("JupyterNotebookInstances", "JupyterNotebookInstance", HasStatusField = true)]
[KubernetesCategoryName("jupyternetes")]
[KubernetesCategoryName("kadense")]
[KubernetesShortName("jni")]
public class JupyterNotebookInstance : KadenseCustomResource
{
    /// <summary>
    /// Specification for this instance
    /// </summary>
    [JsonPropertyName("spec")]
    public JupyterNotebookInstanceSpec? Spec { get; set; }

    /// <summary>
    /// Status for the instance
    /// </summary>
    [JsonPropertyName("status")]
    public JupyterNotebookInstanceStatus Status { get; set; } = new JupyterNotebookInstanceStatus();
}
```

The spec, status and scale fields are defined as normal classes with the JsonPropertyName field to help with serialization.

```csharp
public class JupyterNotebookInstanceSpec
{
    /// <summary>
    /// The notebook template object for this Instance
    /// </summary>
    [JsonPropertyName("template")]
    public NotebookTemplate? Template { get; set; }

    /// <summary>
    /// Variables to be passed to the template for this Instance
    /// </summary>
    [JsonPropertyName("variables")]
    public Dictionary<string, string>? Variables { get;set; }
}
```

There are a number of different attributes in play here which let Kadense know what to do when building the CRD's

| Attribute | Description |
| --- | --- | 
| KubernetesCustomResource | Lets Kadense know the plural and kind values for the resource. |
| KubernetesCategoryName | Lets Kadense know that the custom resources should be queryable using the category name |
| KubernetesShortName | Lets Kadense know that there is a short name for this resource making it queryable using that short name instead |

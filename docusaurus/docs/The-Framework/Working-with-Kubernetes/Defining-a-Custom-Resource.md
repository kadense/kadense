---
title: Defining a Custom Resource
sidebar_position: 2
---

The `KadenseCustomResource` abstract class provides standard Metadata objects, allowing you to focus on defining the `spec`, `status`, and `scale` fields for your resource.

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

The `spec`, `status`, and `scale` fields are defined as normal classes with the `JsonPropertyName` attribute to assist with serialization.

```csharp
public class JupyterNotebookInstanceSpec
{
    /// <summary>
    /// The notebook template object for this instance
    /// </summary>
    [JsonPropertyName("template")]
    public NotebookTemplate? Template { get; set; }

    /// <summary>
    /// Variables to be passed to the template for this instance
    /// </summary>
    [JsonPropertyName("variables")]
    public Dictionary<string, string>? Variables { get; set; }
}
```

Several attributes are used here to guide Kadense in building the CRDs:

| Attribute | Description |
| --- | --- |
| `KubernetesCustomResource` | Specifies the plural and kind values for the resource. |
| `KubernetesCategoryName` | Indicates that the custom resources should be queryable using the category name. |
| `KubernetesShortName` | Specifies a short name for this resource, making it queryable using that short name. |

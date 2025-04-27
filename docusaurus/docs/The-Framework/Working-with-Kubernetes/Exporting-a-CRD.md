---
title: Exporting the CRD
sidebar_position: 3
---

Once we have our custom resource defined we may want we can export these CRDs so that we can install them into helm charts. To do so we can use the **CustomResourceDefinitionFactory** to create the CRD and then serialize using the serializer of your choice:

```csharp
var serializer = new SerializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance) // Use camel case for property names
    .IgnoreFields()
    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull) // Omit null values
    .Build();

var crdFactory = new CustomResourceDefinitionFactory();
var crd = crdFactory.Create<JupyterNotebookInstance>();
crd.ApiVersion = "apiextensions.k8s.io/v1";
crd.Kind = "CustomResourceDefinition";
return serializer.Serialize(crd);
```

Alternatively you may want to install this directly using the kubernetes API:

```csharp
CustomResourceDefinitionFactory crdFactory = new CustomResourceDefinitionFactory();
var crd = crdFactory.Create<TestKubernetesObject>();
var genericClient = new GenericClient(client, "apiextensions.k8s.io","v1","customresourcedefinitions");
var createdCrd = await genericClient.CreateAsync<V1CustomResourceDefinition>(crd);
```

---
title: Polymorphism
sidebar_position: 4
---

Sometimes your models will need lists of objects with a shared heritage 

```yaml
apiVersion: kadense.io/v1
kind: MalleableModule
metadata:
  name: test-polymorph
  namespace: default
spec:
  classes:
    PolymorphicBaseClass:
      discriminatorProperty: "type"
      properties:
        BaseStringProperty:
          type: string
    
    PolymorphicDerivedClass1:
      baseClass: PolymorphicBaseClass
      typeDiscriminator: DerivedString
      properties:
        DerivedStringProperty:
          type: string
    
    PolymorphicDerivedClass2:
      baseClass: PolymorphicBaseClass
      typeDiscriminator: DerivedInt
      properties:
        DerivedIntProperty:
          type: int    
```

The ***MalleableAssemblyBuilder*** will populate the ***MalleableAssembly*** that it generates with the configuration for the JsonSerializer. This can then be passed into the ***MalleablePolymorphicTypeResolver*** and in turn into the ***JsonSerializerOptions*** as follows:

```csharp
public IList<MalleableAssembly> Assemblies { get; }
protected MalleablePolymorphicTypeResolver? TypeResolver { get; set; }

public JsonSerializerOptions GetJsonSerializerOptions()
{
    if(TypeResolver == null)
    {
        TypeResolver = new MalleablePolymorphicTypeResolver();
        foreach (var assembly in Assemblies)
        {
            TypeResolver.MalleableAssembly.Add(assembly);
        }
    }
    var options = new JsonSerializerOptions
    {
        TypeInfoResolver = TypeResolver,
        WriteIndented = true
    };

    return options;
}
```

This can then be called when serializing or deserializing the objects:

```csharp
// serialize to stream
await System.Text.Json.JsonSerializer.SerializeAsync<T>(stream, content, this.GetJsonSerializerOptions());

// deserialize from stream
var message = await JsonSerializer.DeserializeAsync<TMessage>(stream, this.GetJsonSerializerOptions());

// serialize to a HttpResponse stream
await context.Response.WriteAsJsonAsync<T>(content, this.GetJsonSerializerOptions());

// create content for a HttpRequest
var content = JsonContent.Create(instance, instance.GetType(), options: GetJsonSerializerOptions())
```

Of course, when using the [Malleable Workflow Engine](../Malleable-Workflow/Workflow-Engine.md) all of the serializers should implement this by default.
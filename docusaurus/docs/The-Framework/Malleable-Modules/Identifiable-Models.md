---
title: Identifiable Models
sidebar_position: 3
---

In some use cases the classes we may want a way to identify whether an instance of that class is unique or not. To do so, we can add an **identifierExpression**:

```yaml
apiVersion: kadense.io/v1
kind: MalleableModule
metadata:
  name: test-identifable
  namespace: default
spec:
  classes:
    TestClass:
      identifierExpression: Input.TestProperty
      properties:
        TestProperty:
          type: string
```

This will cause the MalleableAssemblyBuilder to add the **IMalleableIdentifiable** interface to the object, and create a method for this based upon the expression defined in the property.

```csharp
var identifiable = (IMalleableIdentifiable)malleableInstance;
identifiable.GetIdentifier()
```

As this is a .Net expression we can concatenate strings together to make a composite key if we need to:

```yaml
apiVersion: kadense.io/v1
kind: MalleableModule
metadata:
  name: test-composite-identifable
  namespace: default
spec:
  classes:
    TestClass:
      identifierExpression: >-
        string.Format("{0}/{1}", Input.TestNamespace, Input.TestProperty) 
      properties:
        TestProperty:
          type: string
        TestNamespace:
          type: string
```

In this instance if you have the following object:

```json
{
  "TestNamespace" : "test-namespace",
  "TestProperty" : "test-property"
}
```

Then once the identifier is called it should return

```text
test-namespace/test-property
```
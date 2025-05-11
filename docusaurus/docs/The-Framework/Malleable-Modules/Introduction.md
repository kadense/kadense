---
title: Introduction
sidebar_position: 1
---

Malleable Models are data models defined via a Custom Resource Definition that can be converted via reflection and emitting into .Net modules. These modules can be used by other software using the Kadense framework for a variety of dynamic applications.


```yaml
apiVersion: kadense.io/v1
kind: MalleableModule
metadata:
  name: test-module
  namespace: default
spec:
  classes:
    TestClass:
      properties:
        TestProperty:
          type: string
    TestClass2:
      baseClass: TestClass
      properties:
        TestProperty2:
          type: list
          subType: string
    TestClass3:
      baseClass: TestClass2
      properties:
        TestProperty3:
          type: list
          subType: TestClass
```

This will essentially create a module which would be the equivalent of the following:

```c#
public class TestClass
{
    public string TestProperty { get; set; }
}

public class TestClass2 : TestClass
{
    public List<string> TestProperty2 { get; set; }
}

public class TestClass3 : TestClass2
{
    public List<TestClass> TestProperty3 { get; set; }
}
```

As a result of this, the applications leveraging Malleable data types can understand a wide range of complex classes that can be dynamically defined.

These types can be used in real-time processing, near real-time processing, batch processing, etc.


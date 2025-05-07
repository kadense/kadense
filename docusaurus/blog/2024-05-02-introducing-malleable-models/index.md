---
slug: introducing-malleable-models
title: Introducing Malleable Models
authors: [shaun]
tags: [malleable,models,events-driven,intro]
---

As part of our ongoing work on the Kadense Framework, we've added in the capability to make Malleable Models and Converters using custom resource definitions.
<!-- truncate -->

So what are Malleable Models? Well these models allow you to define dynamic data types. It works in a similar fashion to the custom resource definitions themselves in Kubernetes. You essentially create a data model in yaml or json and upload it into the Kubernetes control plane:

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

This is then ingested by kadense components and converted into classes in .Net, so the above would essentially become the equivalent of the following:

```c-sharp

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

We've also created conversion modules to assist in converting from one data type to another:

```yaml
apiVersion: kadense.io/v1
kind: MalleableConverterModule
metadata:
  name: converter
  namespace: default
spec:
  converters:
    FromTutorialV1ToTutorialV2:
        from:
            className: TutorialV1
            moduleName: conversion-tutorial
            moduleNamespace: default
        to:
            className: TutorialV2
            moduleName: conversion-tutorial
            moduleNamespace: default
        expressions:
            FirstName: Source.Author.Split(' ')[0]
            Surname: Source.Author.Split(' ')[1]
```

[Full details of Malleable Modules is available in the documentation](https://headinthecloudsolutions.github.io/kadense/docs/The-Framework/Malleable-Modules/Introduction)


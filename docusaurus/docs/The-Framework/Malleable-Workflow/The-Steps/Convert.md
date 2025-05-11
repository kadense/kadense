---
title: Convert Action
sidebar_position: 2
---

Converts from one type to another using a Malleable Converter

```yaml
TestStep:
    action: Convert
    converterOptions:
        converter:
            converterName: FromTestInheritedClassToTestClass
            moduleName: test-converter-module
            moduleNamespace: test-namespace
        nextStep: WriteSuccessApi
```

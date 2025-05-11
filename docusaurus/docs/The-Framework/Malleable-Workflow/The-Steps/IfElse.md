---
title: IfElse Action
sidebar_position: 3
---

Provides conditional splitting of messages into different streams

```yaml
TestConditional:
    action: IfElse
    ifElseOptions:
        expressions:
        - expression: >
            Input.TestString == "test1"
          nextStep: TestStep
        elseStep: WriteFailureApi 
```

you can have multiple expressions and these are processed in the order they are supplied, if none of the conditions are met, then the step named elseStep will be called instead. If no elseStep is provided, the message will be abandoned.
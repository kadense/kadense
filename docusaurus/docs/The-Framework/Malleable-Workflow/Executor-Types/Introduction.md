---
title: Executor Types
sidebar_position: 1
---

By default the workflow engine utilises Akka.Net actors, however you can use other executor types. Each executor type needs to implement a message queue for inbound messages.

```yaml
apiVersion: kadense.io/v1
kind: MalleableWorkflow
metadata:
    namespace: test-namespace
    name: test-workflow
spec:
    description: |
        Test workflow description
    apis:
      InputApi:
        apiType: Ingress
        ingressOptions:
            nextStep: TestConditional
        underlyingType:
            className: TestInheritedClass
            moduleName: test-module
            moduleNamespace: test-namespace
    steps:
      TestConditional:
        action: IfElse
        ifElseOptions:
            expressions:
            - expression: >
                Input.TestString == "test1"
              nextStep: TestStep
            elseStep: WriteFailureApi 
      TestStep:
        action: Convert
        converterOptions:
            converter:
                converterName: FromTestInheritedClassToTestClass
                moduleName: test-converter-module
                moduleNamespace: test-namespace
            nextStep: WriteSuccessApi
        executorType: RabbitMQ    
      WriteSuccessApi:
        action: WriteApi
        options:
            parameters:
                baseUrl: http://localhost:8080/
                path: >
                    "api/success"

      WriteFailureApi:
        action: WriteApi
        options:
            parameters:
                baseUrl: http://localhost:8080/
                path: api/failure


```

In the above definition, the job TestStep is a converter which utilises a RabbitMQ Queue instead of the Akka.Net actor model.

So under default circumstances, the solution will run as follows:

```mermaid
flowchart TB
    a[InputAPI] --> q1([Actor Queue]) --> b{Conditional} -->|When is 'Test1'| q2([Actor Queue]) --> c[Convert Class] --> q3([Actor Queue]) --> d[Write to Success API]
    b --> |Else|q4([Actor Queue]) --> e[Write to Failure API]

    subgraph TestConditional Actor
        q1
        b
    end

    subgraph TestStep actor
        q2
        c
    end

    subgraph WriteSuccessApi actor
        q3
        d
    end

    subgraph WriteSuccessApi actor
        q4
        e
    end
```

Now that we've updated the runner, we have the following instead:

```mermaid
flowchart TB
 subgraph subGraph0["TestConditional Actor"]
        q1(["Actor Queue"])
        b{"Conditional"}
  end
 subgraph subGraph1["WriteSuccessApi actor"]
        q3(["Actor Queue"])
        d["Write to Success API"]
  end
 subgraph subGraph2["WriteSuccessApi actor"]
        q4(["Actor Queue"])
        e["Write to Failure API"]
  end
    a["InputAPI"] --> q1
    q1 --> b
    b -- When is 'Test1' --> q2(["RabbitMQ Queue"])
    q2 --> c["Convert Class"]
    c --> q3
    q3 --> d
    b -- Else --> q4
    q4 --> e

    style q2 fill:#FFD600

```

If this step is a bottleneck for performance we could therefore potentially split it out into it's own individual service:

```mermaid
flowchart TB
    subgraph subGraphActors["Actors"]
        subgraph subGraph0["TestConditional Actor"]
                q1(["Actor Queue"])
                b{"Conditional"}
        end
        subgraph subGraph1["WriteSuccessApi actor"]
                q3(["Actor Queue"])
                d["Write to Success API"]
        end
        subgraph subGraph2["WriteSuccessApi actor"]
                q4(["Actor Queue"])
                e["Write to Failure API"]
        end
    end


    subgraph subGraph3["Individual Service"]
            q2
            c
    end
    a["InputAPI"] --> q1
    q1 --> b
    b -- When is 'Test1' --> q2(["RabbitMQ Queue"])
    q2 --> c["Convert Class"]
    c --> q3
    q3 --> d
    b -- Else --> q4
    q4 --> e

    style q2 fill:#FFD600

```

We could even load balance this service into multiple services fronted by a load balancer:

```mermaid
flowchart TB
    subgraph subGraphActors["Actors"]
        subgraph subGraph0["TestConditional Actor"]
                q1(["Actor Queue"])
                b{"Conditional"}
        end
        subgraph subGraph1["WriteSuccessApi actor"]
                q3(["Actor Queue"])
                d["Write to Success API"]
        end
        subgraph subGraph2["WriteSuccessApi actor"]
                q4(["Actor Queue"])
                e["Write to Failure API"]
        end
    end

    subgraph subGraph3["Individual Service"]
            q2
            c
    end
    subgraph subGraph4["Individual Service"]
            q22(["RabbitMQ Queue"]) -->
            c2["Convert Class"]
    end

    a["InputAPI"] --> q1
    q1 --> b
    b -- When is 'Test1' --> 
    lb[["Load Balancer"]] --> q2(["RabbitMQ Queue"])
    q2 --> c["Convert Class"]
    c --> q3
    q3 --> d
    b -- Else --> q4
    q4 --> e

    lb --> q22
    c2 --> q3

    

    style q2 fill:#FFD600
    style q22 fill:#FFD600

```
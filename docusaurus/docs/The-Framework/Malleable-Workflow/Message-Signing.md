---
title: Message Signing
sidebar_position: 5
---

Kadense signs each message a number of times as it passes through each step of a workflow, each message is signed:

* Against the contents of the message, this is the raw signature.
* Against the raw signature, the step name, and the previous lineage signature for the message. This is the lineage signature.
* Against the raw signature, the step name, and the previous process signature for the processor. This is the process signature.
* Against the lineage signature, the process signature and the lineage ID. This is the combined signature.

In theory if you process a message through the system that are exactly the same and each step is configured the same then you should see exactly the same lineage signature each time it is processed by the time it reaches the end. 

This provides a useful debugging tool that helps you see where a workflow might be diverging if it is creating inconsistent results for whatever reason. It also provides a tamper evident audit that can be assessed if needed.


```mermaid
flowchart TB
        subgraph Lineage1
                direction TB
                subgraph T1
                        a1[raw: abc123..] ~~~ b1[lineage: b1de31..] ~~~ c1[process: h1fanp..] ~~~ d1[combined: def456..]
                end
                subgraph T2
                        a2[raw: abc123..] ~~~ b2[lineage: vasga2..] ~~~ c2[process: fasvbb..] ~~~ d2[combined: def456..]
                end
                subgraph T3
                        a3[raw: 15jp1d..] ~~~ b3[lineage: 5fasmv..] ~~~ c3[process: aqtgb5..] ~~~ d3[combined: def456..]
                end
                subgraph T4
                        a4[raw: 15jp1d..] ~~~ b4[lineage: 1nkl5s..] ~~~ c4[process: 51gsvx..] ~~~ d4[combined: def456..]
                end
        end

        subgraph Lineage2
                direction TB
                subgraph T5
                        a5[raw: abc123..] ~~~ b5[lineage: b1de31..] ~~~ c5[process: nmagm5..] ~~~ d5[combined: def456..]
                end
                subgraph T6
                        a6[raw: abc123..] ~~~ b6[lineage: vasga2..] ~~~ c6[process: m15pon..] ~~~ d6[combined: def456..]
                end
                subgraph T7
                        a7[raw: 15jp1d..] ~~~ b7[lineage: 5fasmv..] ~~~ c7[process: bqn512..] ~~~ d7[combined: def456..]
                end
                subgraph T8
                        a8[raw: 15jp1d..] ~~~ b8[lineage: 1nkl5s..] ~~~ c8[process: 215ng5..] ~~~ d8[combined: def456..]
                end
        end

        T1 --> T2 --> T3 --> T4
        T5 --> T6 --> T7 --> T8

        
        style a1 fill:#ff0,stroke:#333,stroke-width:4px        
        style a2 fill:#ff0,stroke:#333,stroke-width:4px        
        style a5 fill:#ff0,stroke:#333,stroke-width:4px        
        style a6 fill:#ff0,stroke:#333,stroke-width:4px        

        style a3 fill:#0f0,stroke:#333,stroke-width:4px        
        style a4 fill:#0f0,stroke:#333,stroke-width:4px        
        style a7 fill:#0f0,stroke:#333,stroke-width:4px        
        style a8 fill:#0f0,stroke:#333,stroke-width:4px        

        style b1 fill:#0ff,stroke:#333,stroke-width:4px        
        style b2 fill:#0ff,stroke:#333,stroke-width:4px        
        style b3 fill:#0ff,stroke:#333,stroke-width:4px        
        style b4 fill:#0ff,stroke:#333,stroke-width:4px        

        style b5 fill:#0ff,stroke:#333,stroke-width:4px        
        style b6 fill:#0ff,stroke:#333,stroke-width:4px        
        style b7 fill:#0ff,stroke:#333,stroke-width:4px        
        style b8 fill:#0ff,stroke:#333,stroke-width:4px        
```

In the above diagram you've got two identical messages sent into the same basic workflow with an ingress and 3 steps. You can see from that the yellow blocks are are identical, this suggests that the contents of the messages haven't changed between the jobs. 

At some point between the yellow and green steps you can see that the contents have changed. This implies that there has been a conversion or enrichment task run against the data.

You can also see from the diagram that in both lineages, the messages followed exactly the same routes and returned the same data.

## Validating the audit
As we know the order in which each message has come in, and we know the then we should be able to verify the integrity of the audit by replaying all of the messages in the audit and generating the signatures. We don't need the underlying data, just the information in the audit itself.

If someone tampers with an audit message, the signature chains will be broken and this will become evident that someone has tampered with it. This is true whether or not they delete messages from the audit, or if they change the values in an individual audit record.

## Deactivating Message Signing

Obviously there is a cost to signing messages at each step so you can deactivate this functionality if you have no need of it.



```csharp
var builder = System
.AddWorkflow(workflow, malleableAssemblyList)
.WithMessageSigning(false); // Disabled message signing
```

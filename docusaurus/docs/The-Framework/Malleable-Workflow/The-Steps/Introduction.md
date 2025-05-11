---
title: Workflow Steps
sidebar_position: 1
---

Available Step Types are:

| Step Type | Description |
| --- | --- |
| IfElse | Provides conditional splitting of messages into different streams |
| Convert | Converts from one type to another using a Malleable Converter |
| WriteAPI | Writes the contents to an API |

Each workflow step consists of a number of component parts:

## The Queue
The queue is responsible for collecting messages for processing, these are typically fire-and-forget, FIFO (first in/first out) queues. This is important as once a message has been processed and sent on, the processor no longer cares about the message.

By default the Akka provider provides the queue as part of the actor model.

## The Processor
The processor is the big that actually does the work, it will take each message from the queue in turn, do whatever job the processor needs to do and then forward it to whatever destination is being targeted.

## The destinations
The destination lets the workflow know how to send messages to the next step in the chain.

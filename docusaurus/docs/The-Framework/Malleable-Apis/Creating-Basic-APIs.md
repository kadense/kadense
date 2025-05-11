---
title: Creating Basic APIs
sidebar_position: 2
---

You can use the following methodology to create your own Basic API's with any backends you would like by implementing abstract class **MalleableApiBase**.

The following method need to be implemented:

```c#
protected Task ProcessPostAsync<T>(HttpContext context, T content)
{
    // implement your logic for creating the item on the backend
    throw new NotImplementedException();
}
```


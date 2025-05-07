---
title: Creating REST API's
sidebar_position: 4
---

You can use the following methodology to create your own API's with any backends you would like by implementing abstract class MalleableRestApiBase

The following methods need to be implemented:


```c-sharp
protected Task ProcessGetAsync<T>(HttpContext context, string identifier)
{
    // implement your logic for getting the item on the backend
    throw new NotImplementedException();
}

protected Task ProcessPostAsync<T>(HttpContext context, T content)
{
    // implement your logic for creating the item on the backend
    throw new NotImplementedException();
}

protected abstract Task ProcessListAsync<T>(HttpContext context)
{
    // implement your logic for listing the items on the backend
    throw new NotImplementedException();
}

protected abstract Task ProcessPutAsync<T>(HttpContext context, T content, string identifier)
{
    // implement your logic for updating the item on the backend
    throw new NotImplementedException();
}

protected abstract Task ProcessDeleteAsync<T>(HttpContext context, T content, string identifier)
{
    // implement your logic for deleting and item on the backend
    throw new NotImplementedException();    
}
```
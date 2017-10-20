---
layout: default
---

# Method

``` csharp
Task<IRequest> Request();
```

Calling **Request** allows you to move into your next request within the chain and perform a **Get, Put, Post, or Delete**. There are extensions that allow you to skip the Request call and move straight into the request verb, and they can be found **[here](/api/expect/extensions.html)**

**Example**

``` csharp
await new Request()
    .Get("http://localhost:8080/api/todo")
    .Next(x => x.Request())
    .Next(x => x.Delete("http://localhost:8080/api/todo/1"))
    .Done();
```

**[Back to ExpectRequest](/api/expect/expect-request.html)**

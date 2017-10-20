---
layout: default
---

# Method

``` csharp
Task<IExpectRequest> Get(string url);
```

Used to make a Get request to the URL specified.

**Example**

``` csharp
await new Request()
    .Get("http://localhost:8080/api/todo/1")
    .Done();
```

**[Back to Request](/api/request/request.html)**

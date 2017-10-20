---
layout: default
---

# Method

``` csharp
Task<IExpectRequest> Delete(string url);
```

Used to make a Delete request to the URL specified.

**Example**

``` csharp
await new Request()
    .Delete("http://localhost:8080/api/todo/1")
    .Done();
```

**[Back to Request](/api/request/request.html)**

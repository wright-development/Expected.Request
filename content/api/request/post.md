---
title: "Post"
draft: false
---

``` csharp
Task<IExpectRequest> Post(string url, HttpContent);
```

Used to make a POST request to the URL specified. You should use the extension methods to send your object's as HttpContent automatically. View the extension methods **[here](/api/request/extensions)**.

**Example**

``` csharp
await new Request()
    .Post("http://localhost:8080/api/todo/1" new StringContent("..."))
    .Done();
```

**[Back to Request](/api/request)**

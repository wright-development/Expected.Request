---
title: "AddHeader"
draft: false
---

``` csharp
IRequest AddHeader(string key, string value);
```

Allows the user to add headers to the HttpClient that the **Request** requires.

**Example**

``` csharp
await new Request()
    .AddHeader("Authorization", "Bearer Qadz...")
    ...
    .Done();
```

**[Back to Request](/api/request)**

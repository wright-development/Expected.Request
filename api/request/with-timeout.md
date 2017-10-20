---
layout: default
---

# Method

``` csharp
IRequest WithTimeout(TimeSpan span);
```

Used to specify a timeout on the **HttpClient** within the **Request**. In addition, there is an extension method that specifies that there should be no timeout. The method can be found on the extension methods page **[here](/api/request/extensions.html)**.

**Example**

``` csharp
await new Request()
    .WithTimeout(new TimeSpan(10000))
    ...
    .Done();
```


**[Back to Request](/api/request/request.html)**
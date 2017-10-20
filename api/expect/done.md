---
layout: default
---

# Method

``` csharp
Task<IDoneRequest> Done();
```

This completes the Fluent API chain and disposes of the **HttpClient** and **HttpResponse**.

**Example**

``` csharp
await new Request()
    ...
    .Next( x => x.Done()); // This can be shortened to just .Done() using extension methods.
```

**[Back to ExpectRequest](/api/expect/expect-request.html)**

---
layout: default
---

# Method

``` csharp
Task<IExpectRequest> Expect(Action<HttpResponseMessage> expectedAction, string assertionMessage = null);    
Task<IExpectRequest> Expect(Func<HttpResponseMessage, Task> expectedAction, string assertionMessage = null);
```

These are the building blocks for all expectations throughout the framework. The first extension method is for non-async action expectations, and the second is for async expectations. There needs to be a specific function for async expectations which ensures that if an expectation is not met an exception will be properly thrown.

Rely on the extension methods provided, rather than these lower level implementations. The reason is that they provide much higher level support for things like automatic content to object conversion, ability to check status code in a single line, and much more. View the extension methods **[here](/api/expect/extension.html)**

**Example usage**

``` csharp
await new Request()
    .Get("http://localhost:8080/api/todo")
    .Next( x => x.Expect(response => {
            Assert.True(response.StatusCode, HttpStatusCode.OK);
        })
    )
    .Done();

```

**[Back to ExpectRequest](/api/expect/expect-request.html)**

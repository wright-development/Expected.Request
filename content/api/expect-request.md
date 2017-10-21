---
title: "ExpectRequest"
draft: false
---

The **ExpectRequest** class implements the **IExpectRequest** interface and contains the following members.

``` csharp
ExpectRequest(HttpResponseMessage response, HttpClient client)

Task<IRequest> Request();        
Task<IExpectRequest> Expect(Action<HttpResponseMessage> expectedAction, string assertionMessage = null);        
Task<IExpectRequest> Expect(Func<HttpResponseMessage, Task> expectedAction, string assertionMessage = null);        
Task<IDoneRequest> Done();
```

**ExpectRequest** is the assertion piece of the Expected.Request framework. The constructor should rarely, if at all used, instead it should only be implicitly called by the Request class after a request has been made. In addition, the extension methods that are the assertion piece of the framework, which can be found below.

- [Extension Methods](/api/expect-request/extensions) 
- [Request](/api/expect-request/request)
- [Expect](/api/expect-request/expect)
- [Done](/api/expect-request/done)

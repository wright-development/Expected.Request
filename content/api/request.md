---
title: "Request"
draft: false
---

The **Request** class implements the **IRequest** interface and contains the following members.

``` csharp
Request()
Request(HttpClient client)

IRequest AddHeader(string key, string value);
IRequest WithTimeout(TimeSpan span);
Task<IExpectRequest> Post(string url, HttpContent content);
Task<IExpectRequest> Put(string url, HttpContent content);
Task<IExpectRequest> Get(string url);
Task<IExpectRequest> Delete(string url);
```

The purpose of the **Request** class is to construct an object capable of making requests to an API. As a result, there are two constructors, the parameterless constructor and the constructor that receives an HttpClient. The parameterless constructor will create an HttpClient for you, and the constructor that receives an HttpClient will allow you to specify your own. The documentation for the public functions are in the links below.

 - [Extension Methods](/api/request/extensions)
 - [Delete](/api/request/delete)
 - [Get](/api/request/get)
 - [Post](/api/request/post)
 - [Put](/api/request/put)
 - [WithTimeout](/api/request/with-timeout)
 - [AddHeader](/api/request/add-header)
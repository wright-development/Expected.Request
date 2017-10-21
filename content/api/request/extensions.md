---
title: "Request Extensions"
draft: false
---

## Timeout Methods

``` csharp
IRequest NoTimeout(this IRequest request);
```

### Example

``` csharp
new Request()
    .NoTimeout()
    .Get("http://localhost:8080/api/todo")
    ...
    .Done();
```

## Request Methods

``` csharp
Task<IExpectRequest> Post<T>(this IRequest request, string url, T type);
Task<IExpectRequest> Put<T>(this IRequest request, string url, T type);
Task<IExpectRequest> Post<T>(this IRequest request,string url, T type, IContentConverter<T> converter);
Task<IExpectRequest> Put<T>(this IRequest request,string url, T type, IContentConverter<T> converter);
```

By default the **Post** and **Put** will convert your type to JSON, if you would like to convert to a different type then you will need to implement the **IContentCoverter<T>** interface. 

### Examples

``` csharp
new Request()
    .Post("http://localhost:8080/api/todo", new TodoModel {Done=false, Title="Walk the dog."})
    ...
    .Done();

new Request()
    .Put("http://localhost:8080/api/todo", new TodoModel {Done=false, Title="Walk the dog."})
    ...
    .Done();

new Request()
    .Put("http://localhost:8080/api/todo", new TodoModel {Done=false, Title="Walk the dog."}, new XmlContentConventer<TodoModel>())
    ...
    .Done();
```

**[Back to Request](/api/request)**

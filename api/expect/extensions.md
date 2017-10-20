---
layout: default
---

# Extension Methods

## Map Extensions

``` csharp
Task<IExpectRequest> Map<T>(this IExpectRequest expect, Action<T> retrieveObject);
Task<IExpectRequest> Map<T>(this IExpectRequest expect, Action<T> expectedAction, IContentConverter<T> converter)
```

**Map** does exactly what the method name implies, it maps the HttpReponseMessage's content into the object type specified. Then using the Action that the caller provides supplies the type.

## Expect Extensions

``` csharp
Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Func<T,Task> taskFunc, string assertMessage = null);
Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Func<T,Task> taskFunc, IContentConverter<T> converter, string assertMessage = null);

Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Action<T> expectedAction, string assertMessage = null);
Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Action<T> expectedAction, IContentConverter<T> converter, string assertMessage = null); 
```

There are two forms of the **Expect** function, one that takes a function of type T that returns a task and the other simply takes an action of type T. The function is used when you need to pass an async lambda for your assertion against the object of type T. Whereas the action is used for simple assertions that do not require async calls. In both cases when you do not provide an **IContentConverter** by default the code will use the **JsonContentConverter<T>** and map the HttpResponseMessage's content to JSON. Finally, you have the ability to specify the assertion message that you would like to display if your expectation does not complete successfully.


**Example**
``` csharp
class TodoModel
{
    public string Id {get;set;}
    //...
}
//...assertion
await new Request()
    .Get("http://localhost:8080/api/todo/1")
    .Next( x => x.Expect<TodoModel>(model => {
        Assert.Equal(model.Id, "1");
    }))
    .Done();

```

``` csharp
Task<IExpectRequest> ExpectContent(this IExpectRequest expect, string expectedContent);
Task<IExpectRequest> ExpectContent(this IExpectRequest expect, Action<string> expectedContentAction);
```

**ExpectContent** allows the caller to expect the exact content to match, in the case of the first function, or perform a custom action assertion in the case of the second function. 


**Example**
``` csharp
await new Request()
    .Get("http://localhost:8080/api/values/1")
    .Next( x => x.ExpectContent("value"))
    .Done();


await new Request()
    .Get("http://localhost:8080/api/values/1")
    .Next( x => x.ExpectContent( content => {
        Assert.Equal(content, "value")
    }))
    .Done();
```


## Request Extensions

### Retrieve Content Extension


``` csharp
public async static Task<IExpectRequest> GetContent(this IExpectRequest expect, Action<string> contentRetriever);
```

**GetContent** allows the caller to retrieve the value of the HttpResponseMessage's content.

**Example**
``` csharp
string content;
await new Request()
    .Get("http://localhost:8080/api/todo")
    .Next( x => x.GetContent(c => content = c))
    .Done();
```

### Verb Extensions

``` csharp
Task<IExpectRequest> Get(this IExpectRequest expect, string url);
Task<IExpectRequest> Post<T>(this IExpectRequest expect, string url, T content);
Task<IExpectRequest> Put<T>(this IExpectRequest expect, string url, T content);
Task<IExpectRequest> Post(this IExpectRequest expect, string url, HttpContent content);
Task<IExpectRequest> Put(this IExpectRequest expect, string url, HttpContent content);
Task<IExpectRequest> Delete(this IExpectRequest expect, string url);
```

All of the verb functions perform a request against the specified URL provided. These extension method calls remove the need to call **.Request()** first, and simply perform both the request and verb calls.

**Example**
``` csharp
await new Request()
    .Get("http://localhost:8080/api/todo")
    .ExpectOk()    
    .Next( x => x.Delete("http://localhost:8080/api/todo/1"))
    .Done();

var model = new TodoModel{Title = "title"};
await new Request()
    .Get("http://localhost:8080/api/todo")
    .ExpectOk()
    .Next( x => x.Post<TodoModel>("http://localhost:8080/api/todo/1", model))
    .ExpectOk()    
    .Done();
```

## Header Extensions

``` csharp
Task<IExpectRequest> ExpectHeader(this IExpectRequest expect, string headerKey);
Task<IExpectRequest> ExpectHeaders(this IExpectRequest expect, IEnumerable<string> headers);

Task<IExpectRequest> ExpectHeader(this IExpectRequest expect, string headerKey, string headerValue);
Task<IExpectRequest> ExpectHeaders(this IExpectRequest expect, Dictionary<string,string> headers);
```

**ExpectHeader** and **ExpectHeaders** have the ability to either check if a header exists or exist with a specific value.

``` csharp
new Request()
    .Get("http://localhost:8080/api/todo")
    .ExpectHeader("Authorization")
    .Done();    
```


## StatusCode Extensions

``` csharp
Task<IExpectRequest> ExpectStatusCode(this IExpectRequest expect, HttpStatusCode expectedStatusCode);
Task<IExpectRequest> ExpectOk(this IExpectRequest expect);
Task<IExpectRequest> ExpectNotFound(this IExpectRequest expect);
Task<IExpectRequest> ExpectUnavailable(this IExpectRequest expect);
```

**Examples**

``` csharp
new Request()
    .Get("http://localhost:8080/api/todo")
    .ExpectOk()
    .Done();


new Request()
    .Get("http://localhost:8080/api/todo")
    .ExpectStatusCode(HttpStatusCode.OK)
    .Done();    
```


**[Back to ExpectRequest](/api/expect/expect-request.html)**

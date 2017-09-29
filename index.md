---
layout: default
---

![](https://api.travis-ci.org/wright-development/Expected.Request.svg?branch=master)

Just as the name suggests Expected.Request helps you define what you **expect** out of your requests.

Expected Request is a Fluent API that guides you through your API testing. It all starts by creating a new Request.

## Simple usage

``` csharp
var request = new Request();
```

Once you have created a request, let the chaining begin! Currently the there are the four basic requests **Get, Post, Put, and Delete**. Let's try a Post, and we expect to get a status code of ok.

``` csharp
//assuming we have an api setup
var todoItem = new TodoModel {Text = "Walk the dog", done = false };
await request
    .Post("http://localhost:3000/api/todo", todoItem)
    .Next( x => x.ExpectOk())
    .Next( x => x.Done());
```

Simple right? The idea behind Expected.Request is to test your API's in a clear and cohesive way. Essentially you should be able to read your API tests. In this example we create a new request, post to the todo API with our model, then once the post is complete we expect the status code to be ok, and finally, we call done. Calling done at the end ensures that everything is disposed of properly.

## More complex usage

Alright, let's get into a more complex example. We'll post a model to the todo api, then use the id we will retrieve the model from the API, and finally, perform some assertions on the object.

``` csharp
var apiUrl = "http://localhost:3000/api/todo";
var todoItem = new TodoModel {Text = "Walk the dog", done = false };

string id = "";

await new Request()
    .Post(apiUrl, todoItem)
    .Next(x => x.Map<TodoModel>(model=>id = model.Id))
    .Next(x => x.ExpectOk())
    .Next(x => x.Get($"{apiUrl}/{id}"))
    .Next(x => x.ExpectOk())
    .Next(x => x.Expect<TodoModel>(model=> {
        Assert.Equal(model.Id, id);
        Assert.Equal(model.Checked, todoItem.Checked);
        Assert.Equal(model.Text, todoItem.Text);
    }))
    .Next(x => x.Done());
```

Once again with a few calls, we have a fairly self-explanatory API test. 

**If you have any questions or suggestions for this project feel free to post an [issue on github](https://github.com/wright-development/Expected.Request/issues).**
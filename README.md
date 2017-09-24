# Expected.Request

## Example

``` csharp
var ApiUrl = "http://localhost:3000/api/todo";
var content = new TodoModelBuilder().Build();
string id = "";
await new AsyncRequest()
    .Post(ApiUrl, content)
    .Next(x => x.Map<TodoModel>(model=>id = model.Id))
    .Next(x => x.ExpectOk())
    .Next(x => x.Request())
    .Next(x => x.Get($"{ApiUrl}/{id}"))
    .Next(x => x.ExpectOk())
    .Next(x => x.Expect<TodoModel>(model=> {
        model.Id.ShouldBe(id);
        model.Checked.ShouldBe(content.Checked);
        model.Text.ShouldBe(content.Text);
    }))
    .Next(x => x.Done());
```
# Expected.Request

![](https://api.travis-ci.org/wright-development/Expected.Request.svg?branch=master)

## Example Usage

``` csharp
var ApiUrl = "http://localhost:3000/api/todo";
var content = new TodoModelBuilder().Build();
string id = "";
await new Request()
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

## Running Tests

## Unit

**docker-compose -f docker-compose-unit.yml up --build --abort-on-container-exit**

### Integration

**docker-compose -f docker-compose-integration.yml up --build --abort-on-container-exit**
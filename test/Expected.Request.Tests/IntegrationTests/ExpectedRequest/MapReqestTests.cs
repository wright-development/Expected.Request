using Newtonsoft.Json;
using Xunit;
using Expected.Request.Tests.Builders;
using Shouldly;

namespace Expected.Request.Tests.IntegrationTests.ExpectedRequest
{
    public class MapReqestTests : TodoFixture
    {
        [Fact]
        public void should_map_the_response_correctly()
        {
            var content = new TodoModelBuilder().Build();
            string responseContent = "";
            TodoModel actualModel = null;
            new Request()
                .Post<TodoModel>(ApiUrl, content)
                .Map<TodoModel>(model => actualModel = model)
                .Expect((response) => {
                    responseContent = response.Content.ReadAsStringAsync().Result;
                    var expectedModel = JsonConvert.DeserializeObject<TodoModel>(responseContent);
                    expectedModel.ShouldBe(actualModel);
                })
                .Done();

            // await (await (await new Request()
            //     .PostAsync<TodoModel>(ApiUrl, content))
            //     .MapAsync<TodoModel>(model => actualModel = model))
            //     .ExpectAsync((response) => {
            //         responseContent = response.Content.ReadAsStringAsync().Result;
            //         var expectedModel = JsonConvert.DeserializeObject<TodoModel>(responseContent);
            //         expectedModel.ShouldBe(actualModel);
            //     });
        }

    }
}
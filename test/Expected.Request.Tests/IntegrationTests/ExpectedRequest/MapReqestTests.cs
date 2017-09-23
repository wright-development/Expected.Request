using Newtonsoft.Json;
using Xunit;
using Expected.Request.Tests.Builders;
using Shouldly;
using System.Threading.Tasks;
using System;

namespace Expected.Request.Tests.IntegrationTests.ExpectedRequest
{
    public class MapReqestTests : TodoFixture
    {
        [Fact]
        public async void should_map_the_response_correctly()
        {
            var content = new TodoModelBuilder().Build();
            string responseContent = "";
            TodoModel actualModel = null;
            await new AsyncRequest()
                .Post<TodoModel>(ApiUrl, content)
                .Next( x => x.Map<TodoModel>(model=> actualModel = model))
                .Next( x => x.Expect((response)=>{
                    responseContent = response.Content.ReadAsStringAsync().Result;
                    var expectedModel = JsonConvert.DeserializeObject<TodoModel>(responseContent);
                    expectedModel.ShouldBe(actualModel);
                }))
                .Next( x => x.Done());


            // new Request()
            //     .Post<TodoModel>(ApiUrl, content)
            //     .Map<TodoModel>(model => actualModel = model)
            //     .Expect((response) => {
            //         responseContent = response.Content.ReadAsStringAsync().Result;
            //         var expectedModel = JsonConvert.DeserializeObject<TodoModel>(responseContent);
            //         expectedModel.ShouldBe(actualModel);
            //     })
            //     .Done();
        }

    }
}
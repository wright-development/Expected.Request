using System;
using System.Net;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Xunit;
using Shouldly;
using Expected.Request.Exceptions;
using Expected.Request.Tests.Builders;
using System.Threading.Tasks;

namespace Expected.Request.Tests.IntegrationTests
{

    public class GetRequestTests : TodoFixture
    {

        [Fact]
        public async Task should_be_able_to_perform_get_after_post()
        {
            var content = new TodoModelBuilder().Build();
            string id = "";

            await new Request()
                .Post(ApiUrl, content)
                .Next(x => x.Map<TodoModel>(model=>id = model.Id))
                .Next(x => x.ExpectOk())
                .Next(x => x.Get($"{ApiUrl}/{id}"))
                .Next(x => x.ExpectOk())
                .Next(x => x.Expect<TodoModel>(model=> {
                    model.Id.ShouldBe(id);
                    model.Checked.ShouldBe(content.Checked);
                    model.Text.ShouldBe(content.Text);
                }))
                .Done();
        }

        [Fact]
        public async Task should_handle_get_that_returns_no_content()
        {
            await new Request()
                .Get($"{ApiUrl}/{12345}")
                .Next( x => x.ExpectStatusCode(HttpStatusCode.NoContent))
                .Done();
        }

        [Fact]
        public async Task should_throw_exception_if_status_code_does_not_match()
        {
            await Should.ThrowAsync<ExpectedException>(async ()=>{
                await new Request()
                    .Get($"{ApiUrl}/{12345}")
                    .Next(x => x.ExpectStatusCode(HttpStatusCode.OK))
                    .Done();
            });

        }

    }
}

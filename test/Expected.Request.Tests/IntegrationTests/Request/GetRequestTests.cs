using System;
using System.Net;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Xunit;
using Shouldly;
using Expected.Request.Exceptions;
using Expected.Request.Tests.Builders;

namespace Expected.Request.Tests.IntegrationTests
{

    public class GetRequestTests : TodoFixture
    {

        [Fact]
        public void should_be_able_to_perform_get_after_post()
        {
            var content = new TodoModelBuilder().Build();
            string id = "";

            new Request()
                .Post<TodoModel>(ApiUrl, content)
                .Map<TodoModel>(model=>id = model.Id)
                .ExpectOk()
                .Request()
                .Get($"{ApiUrl}/{id}")
                .ExpectOk()
                .Expect<TodoModel>(model=> {
                    model.Id.ShouldBe(id);
                    model.Checked.ShouldBe(content.Checked);
                    model.Text.ShouldBe(content.Text);
                })
                .Done();
        }

        [Fact]
        public void should_handle_get_that_returns_no_content()
        {
            var content = new TodoModelBuilder().Build();

            new Request()
                .Get($"{ApiUrl}/{12345}")
                .ExpectStatusCode(HttpStatusCode.NoContent)
                .Done();
        }

        [Fact]
        public void should_throw_exception_if_status_code_does_not_match()
        {
            Should.Throw<ExpectedException>(()=>{
                new Request()
                    .Get($"{ApiUrl}/{12345}")
                    .ExpectStatusCode(HttpStatusCode.OK)
                    .Done();
            });
        }

    }
}

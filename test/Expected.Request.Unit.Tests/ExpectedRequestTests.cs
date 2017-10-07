using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;
using Expected.Request.Exceptions;
using Expected.Request.Extensions;
using Moq;
using Shouldly;
using Xunit;

namespace Expected.Request.Unit.Tests
{
    public class ExpectRequestTests
    {
        private ExpectRequest _classUnderTest;
        private HttpResponseMessage _response;
        private HttpClient _client;
        private Mock<IContentConverter<Object>> _contentConverter;
        private string _content = "foo_content";


        public ExpectRequestTests()
        {
            _response = new HttpResponseMessage(HttpStatusCode.OK);
            _response.Content = new StringContent(_content);
            _response.Headers.Add("FooHeader", new List<string>{"FooValue"});
            _client = new HttpClient();

            _classUnderTest = new ExpectRequest(_response, _client);     
            _contentConverter = new Mock<IContentConverter<Object>>();
        }


        [Fact]
        public async Task should_throw_an_expected_exception_if_an_exception_is_thrown()
        {
            await Should.ThrowAsync<ExpectedException>(async ()=> {
                await _classUnderTest.Expect((r) => throw new Exception());
            });
        }

        [Fact]
        public async Task should_use_default_message_for_expected_exception_if_custom_not_specified()
        {
            
            var exception = await Should.ThrowAsync<ExpectedException>(async ()=>{
                await _classUnderTest.Expect((r)=> throw new Exception());
            });

            exception.Message.ShouldBe(ExpectRequest.DefaultMessage);
        }

        [Fact]
        public async Task should_use_custom_message_for_expected_exception()
        {
            var errorMessage = "foo message";
            
            var exception = await Should.ThrowAsync<ExpectedException>(async ()=>{
                await _classUnderTest.Expect((r)=> throw new Exception(), errorMessage);
            });

            exception.Message.ShouldBe(errorMessage);
        }

        [Fact]
        public async Task should_use_thrown_expected_exception_message_over_custom_messae()
        {
            
            var errorMessage = "foo message";
            
            var exception = await Should.ThrowAsync<ExpectedException>(async ()=>{
                await _classUnderTest.Expect((r)=> throw new ExpectedException(errorMessage), "foobar");
            });

            exception.Message.ShouldBe(errorMessage);
        }

        [Fact]
        public async Task should_not_throw_an_exception_of_expectation_doesnt_fail()
        {
            await _classUnderTest.Expect((r)=>{});
        }
    }
}
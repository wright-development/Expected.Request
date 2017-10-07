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
        public async Task should_return_content()
        {
            string content = null;
            await _classUnderTest.GetContent((c)=> content = c);
            content.ShouldBe(_content);
        }

        [Fact]
        public async Task should_not_throw_an_exception_of_expectation_doesnt_fail()
        {
            await _classUnderTest.Expect((r)=>{});
        }

        [Fact]
        public async Task should_throw_expected_exception_when_the_status_code_is_incorrect()
        {
            await Should.ThrowAsync<ExpectedException>( async ()=> {
                await _classUnderTest.ExpectStatusCode(HttpStatusCode.NotFound);
            });
        }

        [Fact]
        public async Task should_not_throw_an_exception_when_status_code_is_correct()
        {
            await _classUnderTest.ExpectStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task should_throw_expected_exception_if_header_is_missing()
        {
            await Should.ThrowAsync<ExpectedException>(async () => {
                await _classUnderTest.ExpectHeader("MissingFooHeader");
            });
        }

        [Fact]
        public async Task should_not_throw_an_exception_if_headers_exist()
        {
            await _classUnderTest.ExpectHeader("FooHeader");
        }

        [Fact]
        public async Task should_throw_expected_exception_if_header_value_is_missing()
        {
            await Should.ThrowAsync<ExpectedException>(async () => {
                await _classUnderTest.ExpectHeader("FooHeader", "MissingValue");
            });
        }

        [Fact]
        public async Task should_not_throw_exception_if_header_value_is_present()
        {
            await _classUnderTest.ExpectHeader("FooHeader", "FooValue");
        }

        public async Task should_map_the_object_from_the_content()
        {
            var obj = new {Prop = "Value"};
            _contentConverter.Setup(x => x.ConvertToObject(_content))
                .Returns(obj);

            await _classUnderTest.Expect<Object>((o)=>{
                o.Equals(obj);
            }, _contentConverter.Object);

            _contentConverter.Verify(x => x.ConvertToObject(_content));
        }


        public async Task should_convert_to_object_with_generic_expectation()
        {
            await _classUnderTest.Expect<Object>((o)=>{}, _contentConverter.Object);

            _contentConverter.Verify(x => x.ConvertToObject(_content));
        }

        public async Task should_throw_expected_exception_if_generic_action_fails()
        {
            await Should.ThrowAsync<ExpectedException>(async () => {
                await _classUnderTest.Expect<Object>((o)=>throw new Exception(), _contentConverter.Object);
            });
        }
    }
}
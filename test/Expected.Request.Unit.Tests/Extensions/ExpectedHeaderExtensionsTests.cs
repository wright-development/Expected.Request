using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Exceptions;
using Expected.Request.Extensions;
using static Expected.Request.Extensions.ExpectedHeaderExtensions;
using Shouldly;
using Xunit;

namespace Expected.Request.Unit.Tests.Extensions
{
    public class ExpectedHeaderExtensionsTests
    {
        private ExpectRequest _classUnderTest;

        private HttpResponseMessage _response;
        private HttpClient _client;

        public ExpectedHeaderExtensionsTests()
        {
            _response = new HttpResponseMessage();
            _response.Headers.Add("FooHeader", new List<string>() { "FooValue" });
            _response.Headers.Add("FooBarHeader", new List<string>() { "FooBarValue" });
            _client = new HttpClient();

            _classUnderTest = new ExpectRequest(_response, _client);
        }

        [Fact]
        public async Task should_not_throw_an_exception_if_the_header_exists()
        {
            await _classUnderTest.ExpectHeader("FooHeader");
        }

        [Fact]
        public async Task should_throw_an_exception_if_header_does_not_exist()
        {
            var exception = await Should.ThrowAsync<ExpectedException>(async () =>
            {
                await _classUnderTest.ExpectHeader("NonHeader");
            });

            exception.Message
                .ShouldBe(GetMissingHeadersError(new List<string>{"NonHeader"}));
             
        }

        [Fact]
        public async Task should_not_throw_an_exception_if_all_headers_exist()
        {
            await _classUnderTest.ExpectHeaders(new List<string>{
                "FooHeader",
                "FooBarHeader"
            });
        }

        [Fact]
        public async Task should_throw_an_exception_if_multiple_headers_do_not_exist()
        {
            var exception = await Should.ThrowAsync<ExpectedException>(async () =>
            {
                await _classUnderTest.ExpectHeaders(new List<string>{
                    "NonHeader",
                    "NonHeader2"
                });
            });

            exception.Message
                .ShouldBe(GetMissingHeadersError(new List<string>{"NonHeader", "NonHeader2"}));
            
        }

        [Fact]
        public async Task should_throw_an_exception_if_header_values_do_not_exist()
        {
            var expectedHeaders = new Dictionary<string, string>
                {
                    { "FooHeader", "NoValue" },
                    { "FooBarHeader", "NoBarValue" },
                };

            var exception = await Should.ThrowAsync<ExpectedException>(async () =>
            {
                await _classUnderTest.ExpectHeaders(expectedHeaders);
            });

            exception.Message
                .ShouldBe(GetMissingValuesError(expectedHeaders, _response.Headers));
        }

        [Fact]
        public async Task should_throw_exception_if_headers_and_values_match()
        {
            await _classUnderTest.ExpectHeaders(new Dictionary<string, string>
            {
                { "FooHeader", "FooValue" },
                { "FooBarHeader", "FooBarValue" },
            });
        }
    }
}
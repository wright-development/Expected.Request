using System;
using System.Linq;
using System.Net.Http;
using Shouldly;
using Xunit;

namespace Expected.Request.Unit.Tests
{
    public class RequestTests
    {
        private Request _classUnderTest;
        private HttpClient _client = new HttpClient();


        public RequestTests()
        {
            _classUnderTest = new Request(_client);
        }

        [Fact]
        public void should_add_headers()
        {
            var headerKey ="FooHeader";
            var headerValue = "FooValue";

            _classUnderTest.AddHeader(headerKey,headerValue);

            _client.DefaultRequestHeaders.Contains(headerKey).ShouldBeTrue();
            _client.DefaultRequestHeaders.GetValues(headerKey).First().ShouldBe(headerValue);
        }

        [Fact]
        public void should_update_values_correctly()
        {
            var headerKey ="FooHeader";
            var headerValue = "UpdateFooValue";

            _classUnderTest.AddHeader(headerKey,"FooValue");
            _classUnderTest.AddHeader(headerKey,headerValue);

            _client.DefaultRequestHeaders.Contains(headerKey).ShouldBeTrue();
            _client.DefaultRequestHeaders.GetValues(headerKey).First().ShouldBe(headerValue);
        }

        [Fact]
        public void should_update_timeout_correctly()
        {
            var timeout = new TimeSpan(100);

            _classUnderTest.WithTimeout(timeout);

            _client.Timeout.ShouldBe(timeout);
        }
    }
}
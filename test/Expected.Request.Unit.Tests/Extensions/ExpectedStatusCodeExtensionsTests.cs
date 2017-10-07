using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Exceptions;
using Expected.Request.Extensions;
using static Expected.Request.Extensions.ExpectedStatusCodeExtensions;
using Shouldly;
using Xunit;

namespace Expected.Request.Unit.Tests.Extensions
{
    public class ExpectedStatusCodeExtensionsTests
    {
        private ExpectRequest _classUnderTest;
        private HttpResponseMessage _response;
        private HttpClient _client;

        public ExpectedStatusCodeExtensionsTests()
        {
            _response = new HttpResponseMessage();
            _client = new HttpClient();

            _classUnderTest = new ExpectRequest(_response, _client);
        }

        [Fact]
        public async Task should_not_throw_exception_if_status_code_matches()
        {
            _response.StatusCode = HttpStatusCode.OK;
            await _classUnderTest.ExpectStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task should_throw_exception_if_status_code_does_not_matches()
        {
            _response.StatusCode = HttpStatusCode.OK;
            var exception = await Should.ThrowAsync<ExpectedException>(async ()=>
                await _classUnderTest.ExpectStatusCode(HttpStatusCode.NotFound)
            );

            exception.Message
                .ShouldBe(GetStatusCodeError(HttpStatusCode.NotFound, HttpStatusCode.OK));
        }

        [Fact]
        public async Task should_not_throw_exception_if_status_code_ok_for_expectedok()
        {
            _response.StatusCode = HttpStatusCode.OK;

            await _classUnderTest.ExpectOk();
        }

        [Fact]
        public async Task should_throw_exception_if_status_code_is_not_ok_for_expectedok()
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            var exception = await Should.ThrowAsync<ExpectedException>(async ()=>
                await _classUnderTest.ExpectOk()
            );

            exception.Message
                .ShouldBe(GetStatusCodeError(HttpStatusCode.OK, _response.StatusCode));
        }

         [Fact]
        public async Task should_not_throw_exception_if_status_code_ok_for_expected_not_found()
        {
            _response.StatusCode = HttpStatusCode.NotFound;

            await _classUnderTest.ExpectNotFound();
        }

        [Fact]
        public async Task should_throw_exception_if_status_code_is_not_ok_for_expected_not_found()
        {
            _response.StatusCode = HttpStatusCode.OK;
            var exception = await Should.ThrowAsync<ExpectedException>(async ()=>
                await _classUnderTest.ExpectNotFound()
            );

            exception.Message
                .ShouldBe(GetStatusCodeError(HttpStatusCode.NotFound, _response.StatusCode));
        }

         [Fact]
        public async Task should_not_throw_exception_if_status_code_ok_for_expected_unavailable()
        {
            _response.StatusCode = HttpStatusCode.ServiceUnavailable;

            await _classUnderTest.ExpectUnavailable();
        }

        [Fact]
        public async Task should_throw_exception_if_status_code_is_not_ok_for_expected_unavailable()
        {
            _response.StatusCode = HttpStatusCode.OK;
            var exception = await Should.ThrowAsync<ExpectedException>(async ()=>
                await _classUnderTest.ExpectUnavailable()
            );

            exception.Message
                .ShouldBe(GetStatusCodeError(HttpStatusCode.ServiceUnavailable, _response.StatusCode));
        }
    }
}
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Expected.Request.Converter;
using Moq;
using Xunit;

namespace Expected.Request.Unit.Tests.Extensions
{
    public class RequestExtensionsTests
    {
        private Mock<IRequest> _requestMock = new Mock<IRequest>();
        private string _urlToRequest = "https://localhost:8080";


        [Fact]
        public void should_set_the_timeout_to_infinite_time()
        {
            _requestMock.Object.NoTimeout();

            _requestMock.Verify( x => x.WithTimeout(
                It.Is<TimeSpan>(t => t == Timeout.InfiniteTimeSpan)
            ));
        }

        [Fact]
        public async Task should_execute_done_correct()
        {
            var expectedRequestMock = new Mock<IExpectRequest>();
            _requestMock.Setup( x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(expectedRequestMock.Object));

            await _requestMock.Object.Get("")
                .Done();

            expectedRequestMock.Verify(x => x.Done());
        }

        [Fact]
        public async Task post_should_use_json_content_by_default()
        {
            var obj = new object();

            await _requestMock.Object.Post<Object>(_urlToRequest, obj);

            _requestMock.Verify( x => x.Post(
                _urlToRequest,
                It.Is<StringContent>(s => s.Headers.ContentType.MediaType == new JsonContentConverter<Object>().ContentType)
            ));
        }


        [Fact]
        public async Task put_should_use_json_content_by_default()
        {
            var obj = new object();

            await _requestMock.Object.Put<Object>(_urlToRequest, obj);

            _requestMock.Verify( x => x.Put(
                _urlToRequest,
                It.Is<StringContent>(s => s.Headers.ContentType.MediaType == new JsonContentConverter<Object>().ContentType)
            ));
        }
    }
}
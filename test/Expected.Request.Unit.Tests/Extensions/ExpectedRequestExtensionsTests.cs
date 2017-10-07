using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;
using Moq;
using Xunit;
using Expected.Request.Extensions;
using Expected.Request.Unit.Tests.Converter;

namespace Expected.Request.Unit.Tests.Extensions
{
    public class ExpectedRequestExtensionsTests
    {
        private string _urlToRequest = "https://localhost:8080";
        private Mock<IExpectRequest> _expectedRequestMock = new Mock<IExpectRequest>();
        private Mock<IRequest> _requestMock = new Mock<IRequest>();

        public ExpectedRequestExtensionsTests()
        {
            
        }

        [Fact]
        public async Task should_perform_request_and_get_when_get_is_called()
        {
            SetupRequest();
            
            await _expectedRequestMock.Object.Get(_urlToRequest);

            _expectedRequestMock.Verify( x => x.Request());
            _requestMock.Verify( x => x.Get(_urlToRequest));
        }

        [Fact]
        public async Task should_perform_request_and_post_when_post_is_called()
        {
            SetupRequest();

            await _expectedRequestMock.Object.Post(_urlToRequest, null);

            _expectedRequestMock.Verify(x => x.Request());
            _requestMock.Verify( x => x.Post(_urlToRequest, null));
        }

        [Fact]
        public async Task should_perform_request_and_delete_when_delete_is_called()
        {
            SetupRequest();

            await _expectedRequestMock.Object.Delete(_urlToRequest);

            _expectedRequestMock.Verify(x => x.Request());
            _requestMock.Verify(x => x.Delete(_urlToRequest));
        }

        [Fact]
        public async Task should_perform_request_and_put_when_put_called()
        {
            SetupRequest();

            await _expectedRequestMock.Object.Put(_urlToRequest, It.Is<HttpContent>(x => x == null));        

            _expectedRequestMock.Verify(x => x.Request());
            _requestMock.Verify( x => x.Put( _urlToRequest,null));
        }

        private void SetupRequest(Func<HttpClient, bool> requestCheck = null)
        {
            if(requestCheck == null)
            {
                requestCheck = (HttpClient h) => h == null;
            }

            _expectedRequestMock.Setup(x => x.Request())
                .Returns(Task.FromResult(_requestMock.Object));
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;
using Moq;
using Xunit;

namespace Expected.Request.Unit.Tests
{
    public class ExpectedRequestExtensionsTests
    {
        private string _urlToRequest = "https://localhost:8080";
        private Mock<IExpectRequest> _expectedRequestMock = new Mock<IExpectRequest>();
        private Mock<IRequest> _requestMock = new Mock<IRequest>();


        [Fact]
        public async Task expect_should_convert_content_to_json_by_default()
        {
            SetupRequest();

            await _expectedRequestMock.Object.Expect<Object>((r)=>{});

            _expectedRequestMock.Verify(x => x.Expect<Object>(
                It.IsAny<Action<Object>>(),
                It.IsAny<JsonContentConverter<Object>>()
            ));
        }

        [Fact]
        public async Task map_should_convert_content_to_json_by_default()
        {
            SetupRequest();

            await _expectedRequestMock.Object.Map<Object>((r)=>{});

            _expectedRequestMock.
            Verify(x => x.Map<Object>(
                It.IsAny<Action<Object>>(),
                It.IsAny<JsonContentConverter<Object>>()
            ));
        }

        [Fact]
        public async Task expectok_should_check_that_the_status_is_ok()
        {
            SetupRequest();

            await _expectedRequestMock.Object.ExpectOk();

            _expectedRequestMock.Verify(x => x.ExpectStatusCode(
                It.Is<HttpStatusCode>(s => s == HttpStatusCode.OK)
            ));
        }

        [Fact]
        public async Task should_perform_request_and_get_when_get_is_called()
        {
            SetupRequest();
            
            await _expectedRequestMock.Object.Get(_urlToRequest);

            _expectedRequestMock.Verify( x => x.Request(It.Is<HttpClient>(p=>p == null)));
            _requestMock.Verify( x => x.Get(_urlToRequest));
        }

        [Fact]
        public async Task should_perform_request_and_post_when_post_is_called()
        {
            SetupRequest();

            await _expectedRequestMock.Object.Post(_urlToRequest, null);

            _expectedRequestMock.Verify(x => x.Request(It.Is<HttpClient>(p=>p==null)));
            _requestMock.Verify( x => x.Post(_urlToRequest, null));
        }

        [Fact]
        public async Task should_perform_request_and_delete_when_delete_is_called()
        {
            SetupRequest();

            await _expectedRequestMock.Object.Delete(_urlToRequest);

            _expectedRequestMock.Verify(x => x.Request(It.Is<HttpClient>(p=>p==null)));
            _requestMock.Verify(x => x.Delete(_urlToRequest));
        }

        [Fact]
        public async Task should_perform_request_and_put_when_put_called()
        {
            SetupRequest();

            await _expectedRequestMock.Object.Put(_urlToRequest, It.Is<HttpContent>(x => x == null));        

            _expectedRequestMock.Verify(x => x.Request(It.Is<HttpClient>(p => p==null )));
            _requestMock.Verify( x => x.Put( _urlToRequest,null));
        }

        private void SetupRequest(Func<HttpClient, bool> requestCheck = null)
        {
            if(requestCheck == null)
            {
                requestCheck = (HttpClient h) => h == null;
            }

            _expectedRequestMock.Setup(x => x.Request(null))
                .Returns(Task.FromResult(_requestMock.Object));
        }
    }
}

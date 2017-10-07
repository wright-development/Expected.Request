using System;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;
using Moq;
using Xunit;
using Expected.Request.Extensions;
using static Expected.Request.Extensions.ExpectRequestExtensions;
using Shouldly;
using Expected.Request.Exceptions;

namespace Expected.Request.Unit.Tests.Extensions
{
    public class ExpectedRequestExtensionsTests
    {
        private string _urlToRequest = "https://localhost:8080";
        private string _content = "foo_content";
        private ExpectRequest _classUnderTest;
        private Mock<IExpectRequest> _mockUnderTest = new Mock<IExpectRequest>();
        private Mock<IRequest> _requestMock = new Mock<IRequest>();
        private Mock<IContentConverter<Object>> _contentConverter = new Mock<IContentConverter<Object>>();
        private HttpResponseMessage _response = new HttpResponseMessage();
        private HttpClient _client = new HttpClient();

        public ExpectedRequestExtensionsTests()
        {
            _response.Content = new StringContent(_content);
            _classUnderTest = new ExpectRequest(_response,_client);
        }

        [Fact]
        public async Task should_return_content()
        {
            string content = null;
            await _classUnderTest.GetContent((c)=> content = c);
            content.ShouldBe(_content);
        }

        [Fact]
        public async Task should_not_throw_an_exception_if_the_content_matches()
        {
            await _classUnderTest.ExpectContent(_content);
        }

        [Fact]
        public async Task should_throw_exception_if_content_does_not_match()
        {
            var expectedContent = "not_the_right_content";
            var exception = await Should.ThrowAsync<ExpectedException>(async()=>{
                await _classUnderTest.ExpectContent(expectedContent);
            });

            exception.Message.ShouldBe(GetExpectedContentErrorMessage(expectedContent, _content));
        }

        [Fact]
        public async Task should_throw_expection_if_unable_to_retieve_content()
        {
            var exception = await Should.ThrowAsync<ExpectedException>(async()=>{
                await _classUnderTest.GetContent((content)=> throw new Exception());
            });

            exception.Message.ShouldBe(GetContentErrorMessage());
        }

        [Fact]
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

        [Fact]
        public async Task should_perform_request_and_get_when_get_is_called()
        {
            SetupRequest();
            
            await _mockUnderTest.Object.Get(_urlToRequest);

            _mockUnderTest.Verify( x => x.Request());
            _requestMock.Verify( x => x.Get(_urlToRequest));
        }

        [Fact]
        public async Task should_perform_request_and_post_when_post_is_called()
        {
            SetupRequest();

            await _mockUnderTest.Object.Post(_urlToRequest, null);

            _mockUnderTest.Verify(x => x.Request());
            _requestMock.Verify( x => x.Post(_urlToRequest, null));
        }

        [Fact]
        public async Task should_perform_request_and_delete_when_delete_is_called()
        {
            SetupRequest();

            await _mockUnderTest.Object.Delete(_urlToRequest);

            _mockUnderTest.Verify(x => x.Request());
            _requestMock.Verify(x => x.Delete(_urlToRequest));
        }

        [Fact]
        public async Task should_perform_request_and_put_when_put_called()
        {
            SetupRequest();

            await _mockUnderTest.Object.Put(_urlToRequest, It.Is<HttpContent>(x => x == null));        

            _mockUnderTest.Verify(x => x.Request());
            _requestMock.Verify( x => x.Put( _urlToRequest,null));
        }


        private void SetupRequest(Func<HttpClient, bool> requestCheck = null)
        {
            if(requestCheck == null)
            {
                requestCheck = (HttpClient h) => h == null;
            }

            _mockUnderTest.Setup(x => x.Request())
                .Returns(Task.FromResult(_requestMock.Object));
        }
    }
}

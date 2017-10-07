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
    public class ExpectedRequestMapExtensionsTests
    {
        private string _content = "foo_content";
        private ExpectRequest _classUnderTest;
        private Mock<IContentConverter<Object>> _contentConverter = new Mock<IContentConverter<Object>>();

        public ExpectedRequestMapExtensionsTests()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent(_content);
            _classUnderTest = new ExpectRequest(response, new HttpClient());
        }

        [Fact]
        public async Task should_map_the_object_from_the_content()
        {
            var obj = new {Prop = "Value"};
            _contentConverter.Setup(x => x.ConvertToObject(_content))
                .Returns(obj);

            await _classUnderTest.Map<Object>((o)=>{
                o.Equals(obj);
            }, _contentConverter.Object);

            _contentConverter.Verify(x => x.ConvertToObject(_content));
        }

        
    }
}

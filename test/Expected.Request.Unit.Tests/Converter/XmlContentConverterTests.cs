using System;
using Expected.Request.Converter;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Expected.Request.Unit.Tests.Converter
{

    public class XmlContentConverterTests
    {
        private XmlContentCoverter<FooType> _classUnderTest;
        private string _xmlContent = "<?xml version=\"1.0\"?><FooType xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Foo>bar</Foo></FooType>";
        private FooType _xmlObject = new FooType { Foo = "bar" }; 

        public XmlContentConverterTests()
        {
            _classUnderTest = new XmlContentCoverter<FooType>();
        }

        [Fact]
        public void should_convert_object_to_json()
        {
            var actualContent = _classUnderTest.ConvertToContent(_xmlObject);

            actualContent.Replace("\r", "").Replace("\n", "").ShouldBe(_xmlContent);
        }

        [Fact]
        public void should_convert_content_to_object()
        {
            var actualObject = _classUnderTest.ConvertToObject(_xmlContent);

            actualObject.Foo.ShouldBe(_xmlObject.Foo);
        }
        
        [Fact]
        public void should_have_the_correct_content_type()
        {
            _classUnderTest.ContentType.ShouldBe("application/xml");
        }
    }
}
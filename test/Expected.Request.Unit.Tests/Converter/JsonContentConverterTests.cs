using System;
using Expected.Request.Converter;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Expected.Request.Unit.Tests.Converter
{

    public class JsonContentConverterTests
    {
        private JsonContentConverter<FooType> _classUnderTest;
        private string _jsonContent = "{\"foo\":\"bar\"}";
        private FooType _jsonObject = new FooType { Foo = "bar" }; 

        public JsonContentConverterTests()
        {
            _classUnderTest = new JsonContentConverter<FooType>();
        }

        [Fact]
        public void should_convert_object_to_json()
        {
            var actualContent = _classUnderTest.ConvertToContent(_jsonObject);

            actualContent.ShouldBe(_jsonContent);
        }

        [Fact]
        public void should_convert_content_to_object()
        {
            var actualObject = _classUnderTest.ConvertToObject(_jsonContent);

            actualObject.Foo.ShouldBe(_jsonObject.Foo);
        }

        [Fact]
        public void should_have_the_correct_content_type()
        {
            _classUnderTest.ContentType.ShouldBe("application/json");
        }
    }
}
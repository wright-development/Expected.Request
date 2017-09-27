using Newtonsoft.Json;

namespace Expected.Request.Unit.Tests.Converter
{
    public class FooType
    {
        [JsonProperty("foo")]
        public string Foo {get;set;}
    }
}
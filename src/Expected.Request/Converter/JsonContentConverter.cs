using System;
using Newtonsoft.Json;

namespace Expected.Request.Converter
{
    public class JsonContentConverter<T> : IContentConverter<T>
    {
        public string ContentType => "application/json";
        public string ConvertToContent(T type)
        {
            var content = JsonConvert.SerializeObject(type);
            Console.WriteLine("JSON Content: " + content);
            return content;
        }

        public T ConvertToObject(string content)
        {
            Console.WriteLine("JSON Content: " + content);
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}

using System;
using Newtonsoft.Json;

namespace Expected.Request.Converter
{
    public class JsonContentConverter<T> : IContentConverter<T>
    {
        public string ContentType => "application/json";
        public string ConvertToContent(T type)
        {
            return JsonConvert.SerializeObject(type);
        }

        public T ConvertToObject(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}

using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net.Http.Headers;

namespace Expected.Request.Converter
{
    public interface IContentConverter<T>
    {
        string ContentType {get;}
        T ConvertToObject(string content);
        string ConvertToContent(T type);
    }
}

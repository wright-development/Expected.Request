using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace Expected.Request.Converter
{
    public class XmlContentCoverter<T> : IContentConverter<T>
    {
        public string ContentType => "application/xml";

        public string ConvertToContent(T type)
        {
            string result = "";

            using (var stream = new MemoryStream())
            {
                new XmlSerializer(typeof(T)).Serialize(stream, type);
                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            return result;
        }

        public T ConvertToObject(string content)
        {
            T result = default(T);

            using (var reader = new StringReader(content))
            {
                result = (T)(new XmlSerializer(typeof(T)).Deserialize(reader));
            }

            return result;
        }
    }
}

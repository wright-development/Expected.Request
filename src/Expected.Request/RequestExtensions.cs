using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Expected.Request.Converter;

namespace Expected.Request
{
    public static class RequestExtensions
    {
        public static IExpectRequest Post<T>(this IRequest request, string url, T type) =>
            request.Post<T>(url, type, new JsonContentConverter<T>());
        
        public static IExpectRequest Put<T>(this IRequest request, string url, T type) =>
            request.Put<T>(url, type, new JsonContentConverter<T>());

        public static async Task<IExpectRequest> PostAsync<T>(this IRequest request, string url, T type) =>
            await request.PostAsync<T>(url, type,new JsonContentConverter<T>());
        
        public static async Task<IExpectRequest> PutAsync<T>(this IRequest request, string url, T type) =>
            await request.PutAsync<T>(url, type,new JsonContentConverter<T>());
        
        public static IExpectRequest Post<T>(this IRequest request,string url, T type, IContentConverter<T> converter) =>
            request.PostAsync<T>(url, type, converter).Result;

        public static IExpectRequest Put<T>(this IRequest request,string url, T type, IContentConverter<T> converter) =>
            request.PutAsync<T>(url, type,converter).Result;

        public static async Task<IExpectRequest> PostAsync<T>(this IRequest request,string url, T type, IContentConverter<T> converter) =>
            await request.PostAsync(url, new StringContent(converter.ConvertToContent(type), Encoding.UTF8, converter.ContentType));

        public static async Task<IExpectRequest> PutAsync<T>(this IRequest request,string url, T type, IContentConverter<T> converter) =>
            await request.PutAsync(url, new StringContent(converter.ConvertToContent(type), Encoding.UTF8, converter.ContentType));

    }
}

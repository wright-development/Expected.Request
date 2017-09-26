using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Expected.Request.Converter;
using System;
using System.Threading;

namespace Expected.Request
{
    public static class TaskExtensions
    {
        public static async Task<TResult> Next<T, TResult>(this Task<T> task, Func<T, Task<TResult>> nextTask)
        {
            var result = await task;
            var nextTaskResult = await nextTask(result);
            return nextTaskResult;
        }
    }

    public static class RequestExtensions
    {
        public static IRequest NoTimeout(this IRequest request) =>
            request.WithTimeout(Timeout.InfiniteTimeSpan);

        public static async Task<IExpectRequest> Post<T>(this IRequest request, string url, T type) =>
            await request.Post<T>(url, type,new JsonContentConverter<T>());
        
        public static async Task<IExpectRequest> Put<T>(this IRequest request, string url, T type) =>
            await request.Put<T>(url, type,new JsonContentConverter<T>());
        
        public static async Task<IExpectRequest> Post<T>(this IRequest request,string url, T type, IContentConverter<T> converter) =>
            await request.Post(url, new StringContent(converter.ConvertToContent(type), Encoding.UTF8, converter.ContentType));

        public static async Task<IExpectRequest> Put<T>(this IRequest request,string url, T type, IContentConverter<T> converter) =>
            await request.Put(url, new StringContent(converter.ConvertToContent(type), Encoding.UTF8, converter.ContentType));

    }
}

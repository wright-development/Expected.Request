using System;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;

namespace Expected.Request.Extensions
{
    public static class ExpectRequestExtensions
    {
        public async static Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Action<T> expectedAction, string assertMessage = null) =>
            await expect.Expect(expectedAction, new JsonContentConverter<T>(), assertMessage);

        public async static Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Action<T> expectedAction, IContentConverter<T> converter, string assertMessage = null) => 
            await expect.Expect(async response =>
            {
                var content = await response.Content.ReadAsStringAsync();
                expectedAction(converter.ConvertToObject(content));
            }, assertMessage);


        public async static Task<IExpectRequest> GetContent(this IExpectRequest expect, Action<string> contentRetriever) => 
            await expect.Expect(async response =>
            {
                contentRetriever(await response.Content.ReadAsStringAsync());
            }, $"Unable to retrieve content from response.");

        public async static Task<IExpectRequest> Get(this IExpectRequest expect, string url) => 
            await expect.Request()
                .Next(x => x.Get(url));

        public async static Task<IExpectRequest> Post<T>(this IExpectRequest expect, string url, T content) => 
            await expect.Request()
                .Next(x => x.Post(url, content));

        public async static Task<IExpectRequest> Put<T>(this IExpectRequest expect, string url, T content) => 
            await expect.Request()
                .Next(x => x.Put(url, content));


        public async static Task<IExpectRequest> Post(this IExpectRequest expect, string url, HttpContent content) =>
            await expect.Request()
                .Next(x => x.Post(url, content));

        public async static Task<IExpectRequest> Put(this IExpectRequest expect, string url, HttpContent content) => 
            await expect.Request()
                .Next(x => x.Put(url, content));

        public async static Task<IExpectRequest> Delete(this IExpectRequest expect, string url) =>
            await expect.Request()
                .Next(x => x.Delete(url));
    }

}

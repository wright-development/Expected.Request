using System;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;
using Xunit;

namespace Expected.Request.Extensions
{
    public static class ExpectRequestExtensions
    {
        public static string GetExpectedContentErrorMessage(string expectedContent, string actualContent) =>
            $"The expected content [{expectedContent}], does not match the actual content [{actualContent}].";

        public static string GetContentErrorMessage() => $"Unable to retrieve content from response.";

        public static string GetCustomExpectedContentErrorMessage() => "The custom content expectation failed.";

        public async static Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Func<T,Task> taskAction, IContentConverter<T> converter, string assertMessage = null) =>
            await expect.Expect(async response =>
            {
                var content = await response.Content.ReadAsStringAsync();
                await taskAction(converter.ConvertToObject(content));
            }, assertMessage);

        public async static Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Func<T,Task> taskAction, string assertMessage = null) =>
            await expect.Expect(taskAction, new JsonContentConverter<T>(),assertMessage);


        public async static Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Action<T> expectedAction, string assertMessage = null) =>
            await expect.Expect(expectedAction, new JsonContentConverter<T>(), assertMessage);

        public async static Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Action<T> expectedAction, IContentConverter<T> converter, string assertMessage = null) => 
            await expect.Expect(async response =>
            {
                var content = await response.Content.ReadAsStringAsync();
                expectedAction(converter.ConvertToObject(content));
            }, assertMessage);


        public async static Task<IExpectRequest> ExpectContent(this IExpectRequest expect, string expectedContent)
        {
            string responseContent = null;
            await expect.Expect(async r => responseContent = await r.Content.ReadAsStringAsync());
            return await expect.Expect( _ => Assert.Equal(expectedContent, responseContent), GetExpectedContentErrorMessage(expectedContent, responseContent));
        }

        public async static Task<IExpectRequest> ExpectContent(this IExpectRequest expect, Action<string> expectedContentAction)
        {
            string responseContent = null;
            await expect.Expect(async r => responseContent = await r.Content.ReadAsStringAsync());
            return await expect.Expect( _ => expectedContentAction(responseContent), GetCustomExpectedContentErrorMessage());
        }


        public async static Task<IExpectRequest> GetContent(this IExpectRequest expect, Action<string> contentRetriever) => 
            await expect.Expect(async response =>
            {
                var content = await response.Content.ReadAsStringAsync();
                try{
                    contentRetriever(content);
                }catch (Exception e){
                    throw e;
                }   

            }, GetContentErrorMessage());

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

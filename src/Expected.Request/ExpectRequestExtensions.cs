using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;

namespace Expected.Request
{
    public static class ExpectRequestExtensions
    {
        public async static Task<IExpectRequest> Map<T>(this IExpectRequest expect, Action<T> retrieveObject) =>
            await expect.Map<T>(retrieveObject, new JsonContentConverter<T>());


        public async static Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Action<T> expectedAction) => 
            await expect.Expect(expectedAction, new JsonContentConverter<T>());
        

        public async static Task<IExpectRequest> ExpectOk(this IExpectRequest expect) =>
            await expect.ExpectStatusCode(HttpStatusCode.OK);


        public async static Task<IExpectRequest> Get(this IExpectRequest expect,string url,  HttpClient client = null) 
        {
            return await expect.Request(client)
            .Next(x => x.Get(url));
        }

        public async static Task<IExpectRequest> Post<T>(this IExpectRequest expect,string url, T content,  HttpClient client = null) 
        {
            return await expect.Request(client)
            .Next(x => x.Post(url, content));
        }

        public async static Task<IExpectRequest> Put<T>(this IExpectRequest expect,string url, T content,  HttpClient client = null) 
        {
            return await expect.Request(client)
            .Next(x => x.Put(url, content));
        }

        
        public async static Task<IExpectRequest> Post(this IExpectRequest expect,string url, HttpContent content,  HttpClient client = null) 
        {
            return await expect.Request(client)
            .Next(x => x.Post(url, content));
        }

        public async static Task<IExpectRequest> Put(this IExpectRequest expect,string url, HttpContent content,  HttpClient client = null) 
        {
            return await expect.Request(client)
            .Next(x => x.Put(url, content));
        }

        public async static Task<IExpectRequest> Delete(this IExpectRequest expect,string url,  HttpClient client = null) 
        {
            return await expect.Request(client)
            .Next(x => x.Delete(url));
        }
    }

}

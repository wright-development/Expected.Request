using System;
using System.Threading.Tasks;
using Expected.Request.Converter;

namespace Expected.Request.Extensions
{
    public static class ExpectedMapExtensions
    {
        public async static Task<IExpectRequest> Map<T>(this IExpectRequest expect, Action<T> retrieveObject) =>
            await expect.Map<T>(retrieveObject, new JsonContentConverter<T>());

        public async static Task<IExpectRequest> Map<T>(this IExpectRequest expect, Action<T> expectedAction, IContentConverter<T> converter) =>
            await expect.Expect<T>(expectedAction, converter, $"Unable to map the content ({converter.ContentType}) to the type {typeof(T).ToString()}");
    }
}
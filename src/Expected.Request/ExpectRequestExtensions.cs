using System;
using System.Net;
using System.Threading.Tasks;
using Expected.Request.Converter;

namespace Expected.Request
{
    public static class ExpectRequestExtensions
    {
        public static IExpectRequest Map<T>(this IExpectRequest expect, Action<T> retrieveObject) =>
            expect.Map<T>(retrieveObject, new JsonContentConverter<T>());

        public static Task<IExpectRequest> MapAsync<T>(this IExpectRequest expect, Action<T> retrieveObject) =>
            expect.MapAsync<T>(retrieveObject, new JsonContentConverter<T>());

        public static IExpectRequest Expect<T>(this IExpectRequest expect, Action<T> expectedAction) => 
            expect.Expect(expectedAction, new JsonContentConverter<T>());
        
        public static async Task<IExpectRequest> ExpectAsync<T>(this IExpectRequest expect, Action<T> expectedAction, IContentConverter<T> converter) =>
            await expect.ExpectAsync(expectedAction, new JsonContentConverter<T>());

        public static IExpectRequest ExpectOk(this IExpectRequest expect) =>
            expect.ExpectStatusCode(HttpStatusCode.OK);

    }
}

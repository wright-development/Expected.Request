using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;

namespace Expected.Request
{
    public static class ExpectRequestExtensions
    {
        public static Task<IExpectRequest> Map<T>(this IExpectRequest expect, Action<T> retrieveObject) =>
            expect.Map<T>(retrieveObject, new JsonContentConverter<T>());


        public static Task<IExpectRequest> Expect<T>(this IExpectRequest expect, Action<T> expectedAction) => 
            expect.Expect(expectedAction, new JsonContentConverter<T>());
        

        public static Task<IExpectRequest> ExpectOk(this IExpectRequest expect) =>
            expect.ExpectStatusCode(HttpStatusCode.OK);

    }

}

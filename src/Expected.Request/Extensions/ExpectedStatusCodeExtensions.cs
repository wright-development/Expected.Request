using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Expected.Request.Extensions
{
    public static class ExpectedStatusCodeExtensions
    {
        public static string GetStatusCodeError(HttpStatusCode expected, HttpStatusCode actual) => $"The expected status code is {expected}, the actual status code is {actual}.";

        public async static Task<IExpectRequest> ExpectStatusCode(this IExpectRequest expect, HttpStatusCode expectedStatusCode)
        {
            HttpResponseMessage response = null;
            await expect.Expect(r => response = r);
            return await expect.Expect(_ => Assert.Equal(expectedStatusCode, response.StatusCode), GetStatusCodeError(expectedStatusCode, response.StatusCode));
        }

        public async static Task<IExpectRequest> ExpectOk(this IExpectRequest expect) =>
            await expect.ExpectStatusCode(HttpStatusCode.OK);
        
        public async static Task<IExpectRequest> ExpectNotFound(this IExpectRequest expect) =>
            await expect.ExpectStatusCode(HttpStatusCode.NotFound);

        public async static Task<IExpectRequest> ExpectUnavailable(this IExpectRequest expect) =>
            await expect.ExpectStatusCode(HttpStatusCode.ServiceUnavailable);
        

    }
}
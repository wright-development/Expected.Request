using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Expected.Request.Exceptions;
using Xunit;

namespace Expected.Request.Extensions
{
    public static class ExpectedHeaderExtensions
    {
        public static string GetMissingHeadersError(IEnumerable<string> headers)
        {
            var headersString = string.Join(",", headers.Select(header => header));
            return $"The following header(s) {headersString} are missing from the response message's headers.";
        } 
        public static string GetMissingValuesError (Dictionary<string,string> expectedHeaders, HttpResponseHeaders responseHeaders)
        {
            var missingValuesString = string.Join("","", expectedHeaders.Select(header => $"{header.Key} (Expected: {header.Value}, Actual: {responseHeaders.GetValues(header.Key).FirstOrDefault()})"));
            return $"The following header value(s) {missingValuesString} are missing from the response message's header values";
        } 


        public async static Task<IExpectRequest> ExpectHeader(this IExpectRequest expect, string headerKey) =>
            await expect.ExpectHeaders(new List<string>{headerKey});

        public async static Task<IExpectRequest> ExpectHeaders(this IExpectRequest expect, IEnumerable<string> headers)
        {
            return await expect.Expect(response =>
            {
                var missingHeaders = headers
                    .Where(header => !response.Headers.Contains(header));

                if(missingHeaders.Any())
                {
                    throw new ExpectedException(GetMissingHeadersError(missingHeaders));
                }
            });
        }


        public async static Task<IExpectRequest> ExpectHeader(this IExpectRequest expect, string headerKey, string headerValue) =>
            await expect.ExpectHeaders(new Dictionary<string, string>(){{headerKey, headerValue}});


        public async static Task<IExpectRequest> ExpectHeaders(this IExpectRequest expect, Dictionary<string,string> headers)
        {
            await expect.ExpectHeaders(headers.Keys);

            return await expect.Expect(response =>
            {
                var missingValues = headers
                    .Where(header => !response.Headers.GetValues(header.Key).Contains(header.Value));
                
                if(missingValues.Any())
                {
                    throw new ExpectedException(GetMissingValuesError(missingValues.ToDictionary(x => x.Key, x => x.Value), response.Headers));
                }
            });
        }

    }
}
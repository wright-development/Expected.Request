using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;
using Expected.Request.Exceptions;
using Xunit;

namespace Expected.Request
{
    public class ExpectRequest : IExpectRequest, IDisposable
    {
        private HttpResponseMessage _response;

        public ExpectRequest(HttpResponseMessage response)
        {
            _response = response;
        }

        public IExpectRequest ExpectStatusCode(HttpStatusCode code)
        {
            RethrowOnException(
                () => Assert.Equal(code, _response.StatusCode),
                $"The actual status code {_response.StatusCode} does not match the expected status code {code}."
            );
            return this;
        }

        private void RethrowOnException(Action action, string message)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                throw new ExpectedException(message, e);
            }
        }

        public IExpectRequest Expect(Action<HttpResponseMessage> expectedAction)
        {
            RethrowOnException(
                () => expectedAction(_response),
                "The custom expectation threw an exception."
            );
            return this;
        }

        public async Task<IExpectRequest> ExpectAsync<T>(Action<T> expectedAction, IContentConverter<T> converter)
        {
            var content = await _response.Content.ReadAsStringAsync();
            RethrowOnException(
                () => expectedAction(converter.ConvertToObject(content)),
                "The custom expectation threw an exception."
            );
            return this;
        }

        public IExpectRequest Expect<T>(Action<T> expectedAction, IContentConverter<T> converter) =>
            ExpectAsync<T>(expectedAction, converter).Result;

        public IRequest Request()
        {
            Dispose();
            return new Request();
        }

        public void Dispose()
        {
            _response.Dispose();
        }

        public void Done()
        {
            Dispose();
        }

        public IExpectRequest Map<T>(Action<T> expectedAction, IContentConverter<T> converter)
        {
            return MapAsync<T>(expectedAction, converter).Result;
        }

        public async Task<IExpectRequest> MapAsync<T>(Action<T> expectedAction, IContentConverter<T> converter)
        {
            var content = await _response.Content.ReadAsStringAsync();
            expectedAction(converter.ConvertToObject(content));
            return this;
        }

        public IExpectRequest ExpectHeader(string header)
        {
            RethrowOnException(
                () => Assert.True(_response.Headers.Contains(header)),
                $"The header ${header} was not found in the reponse's headers"
            );
            return this;
        }

        public IExpectRequest ExpectHeader(string header, string value)
        {
            ExpectHeader(header);
            IEnumerable<string> values = null;
            
            RethrowOnException(
                () =>
                {
                    if (_response.Headers.TryGetValues(header, out values))
                    {
                        Assert.Equal(values.First(), value);
                    }
                    else
                    {
                        throw new Exception();
                    }
                },
                $"Unabled to parse the header ${header}'s values"
            );



            return this;
        }
    }
}

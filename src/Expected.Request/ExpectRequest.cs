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
    public class ExpectRequest : IExpectRequest
    {
        private HttpResponseMessage _response;
        private HttpClient _client;
        public static readonly string DefaultMessage = "The custom expectation threw an exception.";

        public ExpectRequest(HttpResponseMessage response, HttpClient client)
        {
            _response = response;
            _client = client;
        }

        public async Task<IExpectRequest> Expect(Action<HttpResponseMessage> expectedAction, string assertMessage = null)
        {
            if(assertMessage == null) 
            {
                assertMessage = DefaultMessage;
            }

            RethrowOnException(
                () => expectedAction(_response),
                assertMessage
            );
            return await Task.FromResult(this);
        }

        public async Task<IRequest> Request()
        {
            _response.Dispose();
            return await Task.FromResult(new Request(_client));
        }

        public void Dispose()
        {
            _client.Dispose();
            _response.Dispose();
        }

        

        private void RethrowOnException(Action action, string message)
        {
            try
            {
                action();
            }
            catch(ExpectedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new ExpectedException(message, e);
            }
        }

        private async Task RethrowOnException(Func<Task> action, string message)
        {
            try
            {
                await action();
            }
            catch(ExpectedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new ExpectedException(message, e);
            }
        }

        public async Task<IExpectRequest> Expect(Func<HttpResponseMessage, Task> expectedAction, string assertMessage = null)
        {
            
            if(assertMessage == null) 
            {
                assertMessage = DefaultMessage;
            }

            await RethrowOnException(
                async () => await expectedAction(_response),
                assertMessage
            );
            return await Task.FromResult(this);
        }

        public async Task<IDoneRequest> Done()
        {
            Dispose();
            return await Task.FromResult(new DoneRequest());
        }
    }
}

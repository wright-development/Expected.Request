using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Expected.Request
{
    public class AsyncRequest : IAsyncRequest
    {
        private HttpClient _client;
        private bool _clientSupplied = false;

        public AsyncRequest() : this(new HttpClient())
        {
    
        }

        public AsyncRequest(HttpClient client)
        {
            if(client == null)
            {
                client = new HttpClient();
            }
            else 
            {
                _clientSupplied = true;
            }

            _client = client;
        }


        public async Task<IExpectAsyncRequest> Delete(string url)
        {
            var response = await _client.DeleteAsync(url).ConfigureAwait(false);
            Dispose();            
            return new ExpectAsyncRequest(response);
        }

        public async Task<IExpectAsyncRequest> Get(string url)
        {
            var response = await _client.GetAsync(url);
            Dispose();            
            return new ExpectAsyncRequest(response);
        }

        public async Task<IExpectAsyncRequest> Post(string url, HttpContent content)
        {
            var response = await _client.PostAsync(url, content);
            Dispose();            
            return new ExpectAsyncRequest(response);
        }

        public async Task<IExpectAsyncRequest> Put(string url, HttpContent content)
        {
            var response = await _client.PutAsync(url, content);
            Dispose();
            return new ExpectAsyncRequest(response);
        }

        public void Dispose()
        {
            if(!_clientSupplied) {
                _client.Dispose();
            }
        }

        public IAsyncRequest AddHeader(string key, string value)
        {
            _client.DefaultRequestHeaders.Add(key,value);
            return this;
        }

        public IAsyncRequest WithTimeout(TimeSpan span)
        {
            _client.Timeout = span;
            return this;
        }

    }

    // public class Request : IRequest
    // {
    //     private bool _clientSupplied = false;
    //     private IAsyncRequest _asyncRequest;

    //     public Request()
    //     {
    //         _asyncRequest = new AsyncRequest();
    //     }

    //     public Request(HttpClient client)
    //     {
    //         _asyncRequest = new AsyncRequest(client);
    //     }

    //     public IRequest AddHeader(string key, string value)
    //     {
    //         _asyncRequest.AddHeader(key,value);
    //         return this;
    //     }

    //     public IRequest WithTimeout(TimeSpan span)
    //     {
    //         _asyncRequest.WithTimeout(span);
    //         return this;
    //     }

    //     public IExpectRequest Delete(string url) => 
    //         _asyncRequest.DeleteAsync(url).Result;

    //     public IExpectRequest Get(string url) => 
    //         _asyncRequest.GetAsync(url).Result;

    //     public IExpectRequest Post(string url, HttpContent content) => 
    //         _asyncRequest.PostAsync(url, content).Result;

    //     public IExpectRequest Put(string url, HttpContent content) => 
    //         _asyncRequest.PutAsync(url, content).Result;

    // }
}

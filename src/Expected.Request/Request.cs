using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Expected.Request
{
    public class Request : IRequest
    {
        private HttpClient _client;
        private bool _clientSupplied = false;

        public Request() : this(new HttpClient())
        {
    
        }

        public Request(HttpClient client)
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


        public async Task<IExpectRequest> Delete(string url)
        {
            var response = await _client.DeleteAsync(url).ConfigureAwait(false);
            Dispose();            
            return new ExpectRequest(response);
        }

        public async Task<IExpectRequest> Get(string url)
        {
            var response = await _client.GetAsync(url);
            Dispose();            
            return new ExpectRequest(response);
        }

        public async Task<IExpectRequest> Post(string url, HttpContent content)
        {
            var response = await _client.PostAsync(url, content);
            Dispose();            
            return new ExpectRequest(response);
        }

        public async Task<IExpectRequest> Put(string url, HttpContent content)
        {
            var response = await _client.PutAsync(url, content);
            Dispose();
            return new ExpectRequest(response);
        }

        public void Dispose()
        {
            if(!_clientSupplied) {
                _client.Dispose();
            }
        }

        public IRequest AddHeader(string key, string value)
        {
            _client.DefaultRequestHeaders.Add(key,value);
            return this;
        }

        public IRequest WithTimeout(TimeSpan span)
        {
            _client.Timeout = span;
            return this;
        }

    }
}

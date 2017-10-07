using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Expected.Request
{
    public class Request : IRequest
    {
        private HttpClient _client;

        public Request() : this(new HttpClient())
        {
    
        }

        public Request(HttpClient client)
        {
            if(client == null)
            {
                client = new HttpClient();
            }

            _client = client;
        }


        public async Task<IExpectRequest> Delete(string url)
        {
            var response = await _client.DeleteAsync(url);
            return new ExpectRequest(response, _client);
        }

        public async Task<IExpectRequest> Get(string url)
        {
            var response = await _client.GetAsync(url);
            return new ExpectRequest(response, _client);
        }

        public async Task<IExpectRequest> Post(string url, HttpContent content)
        {
            var response = await _client.PostAsync(url, content);
            return new ExpectRequest(response, _client);
        }

        public async Task<IExpectRequest> Put(string url, HttpContent content)
        {
            var response = await _client.PutAsync(url, content);
            return new ExpectRequest(response, _client);
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

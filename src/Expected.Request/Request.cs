using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Expected.Request
{
    public class Request : IRequest, IDisposable
    {
        private HttpClient _client;

        public Request() : this(new HttpClient())
        {
    
        }

        public Request(HttpClient client)
        {
            if(client == null) client = new HttpClient();
            _client = client;
        }

        public IRequest AddHeader(string key, string value)
        {
            _client.DefaultRequestHeaders.Add(key,value);
            return this;
        }

        public IExpectRequest Delete(string url) => DeleteAsync(url).Result;

        public IExpectRequest Get(string url) => GetAsync(url).Result;

        public IExpectRequest Post(string url, HttpContent content) => PostAsync(url, content).Result;

        public IExpectRequest Put(string url, HttpContent content) => PutAsync(url, content).Result;

        public async Task<IExpectRequest> DeleteAsync(string url)
        {
            var response = await _client.DeleteAsync(url);
            Dispose();            
            return new ExpectRequest(response);
        }

        public async Task<IExpectRequest> GetAsync(string url)
        {
            var response = await _client.GetAsync(url);
            Dispose();            
            return new ExpectRequest(response);
        }

        public async Task<IExpectRequest> PostAsync(string url, HttpContent content)
        {
            var response = await _client.PostAsync(url, content);
            Dispose();            
            return new ExpectRequest(response);
        }

        public async Task<IExpectRequest> PutAsync(string url, HttpContent content)
        {
            var response = await _client.PutAsync(url, content);
            Dispose();
            return new ExpectRequest(response);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}

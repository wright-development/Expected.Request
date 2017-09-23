using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Expected.Request
{
    // public interface IRequest
    // {
    //     IRequest AddHeader(string key, string value);
    //     IRequest WithTimeout(TimeSpan span);
    //     IExpectRequest Post(string url, HttpContent content);
    //     IExpectRequest Put(string url, HttpContent content);
    //     IExpectRequest Get(string url);
    //     IExpectRequest Delete(string url);

    // }

    public interface IAsyncRequest : IDisposable
    {
        IAsyncRequest AddHeader(string key, string value);
        IAsyncRequest WithTimeout(TimeSpan span);
        Task<IExpectAsyncRequest> Post(string url, HttpContent content);
        Task<IExpectAsyncRequest> Put(string url, HttpContent content);
        Task<IExpectAsyncRequest> Get(string url);
        Task<IExpectAsyncRequest> Delete(string url);
    }
}

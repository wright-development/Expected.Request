using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Expected.Request
{
    public interface IRequest
    {
        IRequest AddHeader(string key, string value);
        IRequest WithTimeout(TimeSpan span);
        Task<IExpectRequest> Post(string url, HttpContent content);
        Task<IExpectRequest> Put(string url, HttpContent content);
        Task<IExpectRequest> Get(string url);
        Task<IExpectRequest> Delete(string url);
    }
}

using System.Net.Http;
using System.Threading.Tasks;

namespace Expected.Request
{
    public interface IRequest
    {
        IRequest AddHeader(string key, string value);
        IExpectRequest Post(string url, HttpContent content);
        IExpectRequest Put(string url, HttpContent content);
        IExpectRequest Get(string url);
        IExpectRequest Delete(string url);

        Task<IExpectRequest> PostAsync(string url, HttpContent content);
        Task<IExpectRequest> PutAsync(string url, HttpContent content);
        Task<IExpectRequest> GetAsync(string url);
        Task<IExpectRequest> DeleteAsync(string url);
    }
}

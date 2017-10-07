using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;

namespace Expected.Request
{

    public interface IExpectRequest : IDisposable
    {
        Task<IRequest> Request();        
        Task<IExpectRequest> Expect(Action<HttpResponseMessage> expectedAction, string assertionMessage = null);        
        Task<IExpectRequest> Expect(Func<HttpResponseMessage, Task> expectedAction, string assertionMessage = null);        
        Task<IDoneRequest> Done();
        
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;

namespace Expected.Request
{

    public interface IExpectRequest : IDisposable
    {
        Task<IExpectRequest> Expect(Action<HttpResponseMessage> expectedAction);
        Task<IExpectRequest> Map<T>(Action<T> expectedAction, IContentConverter<T> converter);
        Task<IExpectRequest> Expect<T>(Action<T> expectedAction, IContentConverter<T> converter);
        
        Task<IExpectRequest> ExpectHeader(string header);
        Task<IExpectRequest> ExpectHeader(string header, string value);
        Task<IExpectRequest> ExpectStatusCode(HttpStatusCode code);

        
        Task<IDoneRequest> Done();
        Task<IRequest> Request(HttpClient client = null);
        
    }
}

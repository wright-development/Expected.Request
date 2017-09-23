using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;

namespace Expected.Request
{

    public interface IExpectAsyncRequest : IDisposable
    {
        Task<IExpectAsyncRequest> Expect(Action<HttpResponseMessage> expectedAction);
        Task<IExpectAsyncRequest> Map<T>(Action<T> expectedAction, IContentConverter<T> converter);
        Task<IExpectAsyncRequest> Expect<T>(Action<T> expectedAction, IContentConverter<T> converter);
        
        Task<IExpectAsyncRequest> ExpectHeader(string header);
        Task<IExpectAsyncRequest> ExpectHeader(string header, string value);
        Task<IExpectAsyncRequest> ExpectStatusCode(HttpStatusCode code);

        
        Task<IDoneRequest> Done();
        Task<IAsyncRequest> Request();
        
    }
    // public interface IExpectRequest
    // {
    
    //     IExpectRequest Expect(Action<HttpResponseMessage> expectedAction);
    //     IExpectRequest ExpectHeader(string header);
    //     IExpectRequest ExpectHeader(string header, string value);
        
    //     IExpectRequest Expect<T>(Action<T> expectedAction, IContentConverter<T> converter);
    //     IExpectRequest Map<T>(Action<T> expectedAction, IContentConverter<T> converter);
    //     IExpectRequest ExpectStatusCode(HttpStatusCode code);

    //     IDoneRequest Done();
    //     IRequest Request();
    // }
}

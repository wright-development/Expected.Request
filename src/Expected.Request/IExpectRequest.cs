using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expected.Request.Converter;

namespace Expected.Request
{
    public interface IExpectRequest
    {
    
        IExpectRequest Expect(Action<HttpResponseMessage> expectedAction);
        Task<IExpectRequest> ExpectAsync(Action<HttpResponseMessage> expectedAction);
        IExpectRequest ExpectHeader(string header);
        IExpectRequest ExpectHeader(string header, string value);
        
        IExpectRequest Expect<T>(Action<T> expectedAction, IContentConverter<T> converter);
        IExpectRequest Map<T>(Action<T> expectedAction, IContentConverter<T> converter);
        Task<IExpectRequest> MapAsync<T>(Action<T> expectedAction, IContentConverter<T> converter);
        Task<IExpectRequest> ExpectAsync<T>(Action<T> expectedAction, IContentConverter<T> converter);
        IExpectRequest ExpectStatusCode(HttpStatusCode code);

        void Done();
        IRequest Request();
    }
}

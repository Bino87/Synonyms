using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.Core.Responses;

namespace Synonyms_Test.Core.Implementation;

internal sealed class ResponseFactory : IResponseFactory
{
    private readonly IResponseErrorHandler _responseErrorHandler;

    public ResponseFactory(IResponseErrorHandler responseErrorHandler)
    {
        _responseErrorHandler = responseErrorHandler;
    }

    public Response CreateResponse()
    {
        //create response, it contains error codes so that front end can display appropriate messages
        return new Response(_responseErrorHandler.GetErrorCodes());
    }

    public Response CreateResponse<T>(T? value)
    {
        //create response with values and error codes. 
        return new ValueResponse<T>(_responseErrorHandler.GetErrorCodes(), value);
    }
}
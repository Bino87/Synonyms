using Synonyms_Test.Core.Responses;

namespace Synonyms_Test.Core.Interfaces;

public interface IResponseFactory
{
    Response CreateResponse();

    Response CreateResponse<T>(T? value);
}
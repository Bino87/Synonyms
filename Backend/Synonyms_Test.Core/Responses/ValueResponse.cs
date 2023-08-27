namespace Synonyms_Test.Core.Responses;

public sealed class ValueResponse<T> : Response
{
    public T? Response { get; set; }

    internal ValueResponse(int[] errorCodes, T? response) : base(errorCodes) => Response = response;
}
using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Implementation;

internal sealed class ResponseErrorHandler : IResponseErrorHandler
{
    private readonly ISet<int> _errorCodes = new HashSet<int>(); // hashset because we do not want to have multiple instances of the same error code.
    public void SetErrorCode(int errorCode)
    {
        //add error code to the set, 
        _errorCodes.Add(errorCode);
    }

    public int[] GetErrorCodes()
    {
        return _errorCodes.ToArray(); //parse error codes to an array and return.
    }
}
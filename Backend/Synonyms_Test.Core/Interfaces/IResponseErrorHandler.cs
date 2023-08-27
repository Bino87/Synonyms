namespace Synonyms_Test.Core.Interfaces;

public interface IResponseErrorHandler
{
    void SetErrorCode(int errorCode);
    int[] GetErrorCodes();
}
namespace Synonyms_Test.Core.ErrorHandling;

public static class ErrorCodes
{
    public const int ValidationFailedBecauseWordHadNoValue = 1;
    public const int ValidationFailedBecauseWordContainerIllegalCharacters = 2;
    public const int UnableToRetrieveData = 3;
    public const int WordAlreadyExists = 4;
    public const int WordDoesNotExist = 5;
    public const int SynonymDoesNotExists = 6;
}
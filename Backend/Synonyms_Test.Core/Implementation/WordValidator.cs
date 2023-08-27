using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Synonyms_Test.Core.ErrorHandling;
using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Implementation;

internal sealed class WordValidator : IWordValidator
{
    private const string OnlyLettersPattern = @"^[\p{L}]*$";
    private const string IgnoreLettersPattern = @"[\p{L}]";
    private const string ValidationMessageFormat = "Validation of a word '{0}' failed because of following characters: {1}";
    private const string ValidationFailedBecauseTheInputHasNoValue = "Validation failed because the input has no value!";
    private const string Separator = ", ";

    private readonly ILogger<WordValidator> _logger;
    private readonly IResponseErrorHandler _responseErrorHandler;

    public WordValidator(ILogger<WordValidator> logger, IResponseErrorHandler responseErrorHandler)
    {
        _logger = logger;
        _responseErrorHandler = responseErrorHandler;
    }

    public bool Validate(string? word)
    {
        //check if a word is null or empty
        if (string.IsNullOrWhiteSpace(word))
        {
            _logger.LogInformation(ValidationFailedBecauseTheInputHasNoValue);
            _responseErrorHandler.SetErrorCode(ErrorCodes.ValidationFailedBecauseWordHadNoValue);
            return false;
        }

        //Check if word contains invalid characters, only letters are allowed. 
        if (!Regex.IsMatch(word, OnlyLettersPattern))
        {
            var nonLetterCharacters = Regex.Replace(word, IgnoreLettersPattern, string.Empty);
            var uniqueCharacters = nonLetterCharacters.Distinct().ToArray();
            _logger.LogInformation(string.Format(ValidationMessageFormat, word, string.Join(Separator, uniqueCharacters)));
            _responseErrorHandler.SetErrorCode(ErrorCodes.ValidationFailedBecauseWordContainerIllegalCharacters);
            return false;
        }

        return true;
    }
}
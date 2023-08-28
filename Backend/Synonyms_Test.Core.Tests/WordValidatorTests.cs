using Microsoft.Extensions.Logging;
using NSubstitute;
using Synonyms_Test.Core.ErrorHandling;
using Synonyms_Test.Core.Implementation;
using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Tests;

public class WordValidatorTests
{
    private IWordValidator _service;
    private ILogger<WordValidator> _logger;
    private IResponseErrorHandler _responseErrorHandler;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<WordValidator>>();
        _responseErrorHandler = Substitute.For<IResponseErrorHandler>();
        _service = new WordValidator(_logger, _responseErrorHandler);
    }

    

    [TestCase]
    public void Validate_Fails_WhenInputIsNull()
    {
        //Arrange
        string? input = null;

        //Act
        var res = _service.Validate(input);

        //Assert
        Assert.IsFalse(res);
        _logger.Received(1).Log(LogLevel.Information, 0, "Validation failed because the input has no value!");
        _responseErrorHandler.Received(1).SetErrorCode(ErrorCodes.ValidationFailedBecauseWordHadNoValue);
    }

    [TestCase]
    public void Validate_Fails_WhenInputEmpty()
    {
        //Arrange
        string? input = string.Empty;

        //Act
        var res = _service.Validate(input);

        //Assert
        Assert.IsFalse(res);
        _logger.Received(1).Log(LogLevel.Information, 0, "Validation failed because the input has no value!");
        _responseErrorHandler.Received(1).SetErrorCode(ErrorCodes.ValidationFailedBecauseWordHadNoValue);
    }

    [TestCase]
    public void Validate_Fails_WhenInputContainsNonLetterCharacters()
    {
        //Arrange
        string? input = "123^*";

        //Act
        var res = _service.Validate(input);

        //Assert
        Assert.IsFalse(res);
        _logger.Received(1).Log(LogLevel.Information, 0, "Validation of a word '123^*' failed because of following characters: 1, 2, 3, ^, *");
        _responseErrorHandler.Received(1).SetErrorCode(ErrorCodes.ValidationFailedBecauseWordContainerIllegalCharacters);
    }

    [TestCase]
    public void Validate_Fails_WhenInputContainsLettersAndOtherCharacters()
    {
        //Arrange
        string? input = "abc123^*xyz";

        //Act
        var res = _service.Validate(input);

        //Assert
        Assert.IsFalse(res);
        _logger.Received(1).Log(LogLevel.Information, 0, "Validation of a word 'abc123^*xyz' failed because of following characters: 1, 2, 3, ^, *");
        _responseErrorHandler.Received(1).SetErrorCode(ErrorCodes.ValidationFailedBecauseWordContainerIllegalCharacters);
    }

    [TestCase]
    public void Validate_Passes_WhenInputContainsOnlyLetterCharacters()
    {
        //Arrange
        string? input = "öåäqjhaksjdhkad";

        //Act
        var res = _service.Validate(input);

        //Assert
        Assert.IsTrue(res);
    }

}
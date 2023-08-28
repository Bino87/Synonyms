using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.DataAccess.Interfaces;
using Synonyms_Test.Services.Implementation;
using Synonyms_Test.Services.Interfaces;
using NSubstitute;
using Synonyms_Test.Core.ErrorHandling;
using Synonym_Test.Models;
using Newtonsoft.Json.Linq;

namespace Synonyms_Test.Services.Tests;
public class SynonymServiceTests
{
    private const string Word = "word";
    private const int SynonymId = 1;

    private ISynonymService _service;
    private ISynonymsRepository _synonymsRepository;
    private IWordAdapter _wordAdapter;
    private ISettings _settings;
    private IWordValidator _wordValidator;
    private ILogger<SynonymService> _logger;
    private IResponseErrorHandler _responseErrorHandler;

    [SetUp]
    public void Setup()
    {

        _synonymsRepository = Substitute.For<ISynonymsRepository>();
        _wordAdapter = Substitute.For<IWordAdapter>();
        _settings = Substitute.For<ISettings>();
        _wordValidator = Substitute.For<IWordValidator>();
        _logger = Substitute.For<ILogger<SynonymService>>();
        _responseErrorHandler = Substitute.For<IResponseErrorHandler>();

        _service = new SynonymService(_synonymsRepository, _wordAdapter, _settings, _wordValidator, _logger, _responseErrorHandler);
    }

    #region AddWordAsync Tests

    [TestCase]
    public async Task AddWordAsync_WhenValidationFailsDoesNothing()
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(false);
        //Act
        await _service.AddWordAsync(Word, null);

        //Assert
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);
    }

    [TestCase]
    public async Task AddWordAsync_WhenWordAlreadyExistsReportsErrorAndDoesNothing()
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(true);
        _synonymsRepository.HasWordAsync(Word).Returns(true);
        //Act
        await _service.AddWordAsync(Word, null);

        //Assert
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);

        await _synonymsRepository.Received(1).HasWordAsync(Word);
        _responseErrorHandler.Received(1).SetErrorCode(ErrorCodes.WordAlreadyExists);
    }

    [TestCase]
    public async Task AddWordAsync_WhenWordDoesNotExistsReportsErrorAndDoesNothing()
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(true);
        _synonymsRepository.HasWordAsync(Word).Returns(false);
        //Act
        await _service.AddWordAsync(Word, null);

        //Assert
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);

        await _synonymsRepository.Received(1).HasWordAsync(Word);
    }

    [TestCase]
    public async Task AddWordAsync_WhenSendingNullSynonymValueAddsWordWithoutSynonym()
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(true);
        _synonymsRepository.HasWordAsync(Word).Returns(false);
        //Act
        await _service.AddWordAsync(Word, null);

        //Assert
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);

        await _synonymsRepository.Received(1).HasWordAsync(Word);
    }

    [TestCase]
    public async Task AddWordAsync_WhenSendingSynonymValue_ButItDoesNotExistsReportsErrorAndAborts()
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(true);
        _synonymsRepository.HasWordAsync(Word).Returns(false);
        _synonymsRepository.HasWordWithIdAsync(SynonymId).Returns(false);

        //Act
        await _service.AddWordAsync(Word, SynonymId);

        //Assert
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);
        await _synonymsRepository.Received(1).HasWordWithIdAsync(SynonymId);
        await _synonymsRepository.Received(1).HasWordAsync(Word);
        _responseErrorHandler.Received(1).SetErrorCode(ErrorCodes.SynonymDoesNotExists);
    }

    [TestCase]
    public async Task AddWordAsync_WhenSendingSynonymValue_AndItExistsReportsErrorAndAborts()
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(true);
        _synonymsRepository.HasWordAsync(Word).Returns(false);
        _synonymsRepository.HasWordWithIdAsync(SynonymId).Returns(true);

        //Act
        await _service.AddWordAsync(Word, SynonymId);

        //Assert
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);
        await _synonymsRepository.Received(1).HasWordWithIdAsync(SynonymId);
        await _synonymsRepository.Received(1).HasWordAsync(Word);
        await _synonymsRepository.Received(1).AddWordAsync(Word, SynonymId);
    }

    #endregion

    #region GetAllWordsAsync Tests

    [TestCase]
    public async Task GetAllWordsAsync()
    {
        //Arrange
        var result = new List<Word>();
        _synonymsRepository.GetAllWordsAsync().Returns(result);

        //Act
        var res = await _service.GetAllWordsAsync();

        //Assert
        await _synonymsRepository.Received(1).GetAllWordsAsync();

        Assert.That(res, Is.EqualTo(result));
    }

    #endregion

    #region GetSynonymsAsync Tests

    [TestCase]
    public async Task GetSynonymsAsync_WhenValidationFailsReturnEmptyArray()
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(false);

        //Act
        ICollection<GraphSearchResults> res = await _service.GetSynonymsAsync(Word);

        //Assert
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);
        Assert.That(res, Is.EquivalentTo(Array.Empty<GraphSearchResults>()));
    }

    [TestCase]
    public async Task GetSynonymsAsync_WhenWordWeAreLookingForDoesNotExistsReportErrorReturnEmptyArray()
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(true);
        _synonymsRepository.HasWordAsync(Word).Returns(false);

        //Act
        ICollection<GraphSearchResults> res = await _service.GetSynonymsAsync(Word);

        //Assert
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);
        await _synonymsRepository.Received(1).HasWordAsync(Word);
        _responseErrorHandler.Received(1).SetErrorCode(ErrorCodes.WordDoesNotExist);
        Assert.That(res, Is.EquivalentTo(Array.Empty<GraphSearchResults>()));
    }

    [TestCase]
    public async Task GetSynonymsAsync_WhenUnableToRetrieveTheWordAbort()
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(true);
        _synonymsRepository.HasWordAsync(Word).Returns(true);
        _synonymsRepository.GetWordWithAllSynonymsByValueAsync(Word, Arg.Any<int>()).Returns(default(Word?));

        //Act
        ICollection<GraphSearchResults> res = await _service.GetSynonymsAsync(Word);

        //Assert
        _settings.Received(1).GetSearchDepthLimit();
        await _synonymsRepository.Received(1).GetWordWithAllSynonymsByValueAsync(Word, Arg.Any<int>());
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);
        await _synonymsRepository.Received(1).HasWordAsync(Word);
        Assert.That(res, Is.EquivalentTo(Array.Empty<GraphSearchResults>()));
    }

    [TestCase("ABCDEF", 'A', "BCDEF")]
    [TestCase("ABCDEF", 'B', "ACDEF")]
    [TestCase("ABCDEF", 'C', "BDAEF")]
    [TestCase("ABCDEF", 'D', "CEBFA")]
    [TestCase("ABCDEF", 'E', "DFCBA")]
    [TestCase("ABCDEF", 'F', "EDCBA")]
    public async Task GetSynonymsAsync_WhenWordIsFound(string words, char character, string expected)
    {
        //Arrange
        _wordAdapter.Adapt(Word).Returns(Word);
        _wordValidator.Validate(Word).Returns(true);
        _synonymsRepository.HasWordAsync(Word).Returns(true);
        _synonymsRepository.GetWordWithAllSynonymsByValueAsync(Word, Arg.Any<int>()).Returns(CreateWord(words, character));

        //Act
        ICollection<GraphSearchResults> res = await _service.GetSynonymsAsync(Word);

        //Assert
        _settings.Received(1).GetSearchDepthLimit();
        await _synonymsRepository.Received(1).GetWordWithAllSynonymsByValueAsync(Word, Arg.Any<int>());
        _wordAdapter.Received(1).Adapt(Word);
        _wordValidator.Received(1).Validate(Word);
        await _synonymsRepository.Received(1).HasWordAsync(Word);

        Assert.That(res.Count, Is.EqualTo(expected.Length));

        var array = res.ToArray();

        for (int i = 0; i < res.Count; i++)
        {
            Assert.That(array[i].Word, Is.EqualTo(expected[i].ToString()));
        }
    }


    private static Word CreateWord(string value, char selectedWord)
    {
        int selectedIndex = -1;
        Word[] words = new Word[value.Length];

        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            if (letter == selectedWord) selectedIndex = i;
            var word = new Word()
            {
                Synonyms = new HashSet<Word>(),
                Value = letter.ToString(),
                WordId = i
            };
            words[i] = word;
        }

        for (int i = 0; i < words.Length; i++)
        {
            if (i > 0)
            {
                words[i - 1].Synonyms.Add(words[i]);
            }

            if (i < words.Length - 1)
            {
                words[i + 1].Synonyms.Add(words[i]);
            }
        }


        if(selectedIndex == -1) Assert.Fail("Selected index not found");

        return words[selectedIndex];
    }

    #endregion
}
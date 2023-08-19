using NSubstitute;
using Services.Interfaces;
using Services.Services;
using Shared.Models;

namespace Services.Tests;

public class SynonymServiceTests
{
    private const string Word = "word";
    private const string Synonym = "synonym";
    private const string NewWord = "new word";
    private ISynonymService _service = null!;
    private ISynonymRepository _synonymRepository = null!;

    [SetUp]
    public void Setup()
    {
        _synonymRepository = Substitute.For<ISynonymRepository>();
        _service = new SynonymService(_synonymRepository);
    }

    [TestCase]
    public void WhenCalling_AddWordWithSynonym_ItPassesDataToSynonymRepository()
    {
        //Act
        _service.AddWordWithSynonym(NewWord, Synonym);

        //Assert
        _synonymRepository.Received().InsertWord(NewWord, Synonym);
    }

    [TestCase]
    public void WhenCalling_GetSynonyms_ItPassesDataToSynonymRepository_And_ReturnsSynonymListFromThisRepository()
    {
        //Arrange
        ICollection<WordModel> words = new[]
        {
            new WordModel()
            {
                Value = Synonym
            }
        };

        _synonymRepository.GetSynonyms(Word).Returns(words);

        //Act
        var res = _service.GetSynonyms(Word);

        //Assert
        Assert.That(res, Is.EquivalentTo(words));
        _synonymRepository.Received().GetSynonyms(Word);
    }

    [TestCase]
    public void GetAllWords_ReturnsValuesOfSynonymRepository()
    {
        //Arrange
        ICollection<WordModel> words = new List<WordModel>()
        {
            new()
            {
                Value = Word,
                Id = Guid.NewGuid()
            }
        };
        _synonymRepository.GetAllWords().Returns(words);

        //Act
        var res = _service.GetAllWords();

        //Assert
        Assert.That(res, Is.EquivalentTo(words));
        _synonymRepository.Received(1).GetAllWords();
    }

}
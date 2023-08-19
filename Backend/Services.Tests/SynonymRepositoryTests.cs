using NSubstitute;
using Services.Interfaces;
using Services.Repositories;
using Shared.Models;

namespace Services.Tests;

public class SynonymRepositoryTests
{
    private const string Synonym = "synonym";
    private const string NewWord = "new word";
    private static readonly Guid Id = Guid.NewGuid();

    private ISynonymRepository _service = null!;
    private ISynonymData _synonymData = null!;

    [SetUp]
    public void Setup()
    {
        _synonymData = Substitute.For<ISynonymData>();
        _service = new SynonymRepository( _synonymData);
    }

    #region GetSynonyms Tests

    [TestCase]
    public void When_WordDoesNotExists_ReturnsEmptySynonymCollection_AndAppendsErrorCode()
    {
        //Arrange
        WordContainer? container = null;
        _synonymData.GetContainer(NewWord).Returns(container);

        //Act
        var res = _service.GetSynonyms(NewWord);

        //Assert
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Any(), Is.False);
        _synonymData.Received(1).GetContainer(NewWord);
    }

    [TestCase]
    public void When_WordExists_ButHasNoSynonyms_ReturnsEmptyCollection()
    {
        //Arrange
        var synonymIds = new HashSet<Guid> {Id};
        var word = new WordModel()
        {
            Id = Id,
            Value = NewWord
        };
        var container = new WordContainer()
        {
            WordModel = word,
            SynonymIds = synonymIds
        };
        _synonymData.GetWordsByIds(synonymIds).Returns(new[] { word });
        _synonymData.GetContainer(NewWord).Returns(container);

        //Act
        var res = _service.GetSynonyms(NewWord);

        //Assert
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Any(), Is.False);
        _synonymData.Received(1).GetContainer(NewWord);
        _synonymData.Received(1).GetWordsByIds(synonymIds);
    }

    [TestCase]
    public void When_WordExists_ButHasNoSynonyms_ReturnsEmptyCollection1()
    {
        //Arrange
        var synonymIds = new HashSet<Guid>(){Id};
        var word = new WordModel()
        {
            Id = Guid.NewGuid(),
            Value = NewWord
        };
        var container = new WordContainer()
        {
            WordModel = word,
            SynonymIds = synonymIds
        };
        _synonymData.GetWordsByIds(synonymIds).Returns(new[] { word, new WordModel(){Id = Guid.NewGuid(), Value = Synonym} });
        _synonymData.GetContainer(NewWord).Returns(container);

        //Act
        var res = _service.GetSynonyms(NewWord);

        //Assert
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Any(), Is.True);
        _synonymData.Received(1).GetContainer(NewWord);
        _synonymData.Received(1).GetWordsByIds(synonymIds);
    }

    #endregion

    #region InsertWord Tests
    [TestCase]
    public void When_SynonymData_HasTheWordWeAreTryingToAdd_ReportsErrorAndDoesNothing()
    {
        //Arrange
        _synonymData.ContainsWord(NewWord).Returns(true);

        //Act
        _service.InsertWord(NewWord, Synonym);

        //Assert
        _synonymData.Received(1).ContainsWord(NewWord);
    }

    [TestCase]
    public void When_SynonymData_TriesToAddWordWithValidSynonym_NewCollectionOfSynonymsIsCreated_AndNoAttemptIsMadeToRetrieveSynonymCollection()
    {
        //Arrange
        _synonymData.ContainsWord(NewWord).Returns(false);

        //Act
        _service.InsertWord(NewWord, null);

        //Assert
        _synonymData.Received(1).ContainsWord(NewWord);
        _synonymData.Received(1).AddContainer(Arg.Any<WordContainer>());
    }

    [TestCase]
    public void When_SynonymData_TriesToAddWordWithValidSynonym_CollectionOfThoseSynonymsIsTransferredToThatNewWord()
    {
        //Arrange

        var container = new WordContainer();
        _synonymData.ContainsWord(NewWord).Returns(false);
        _synonymData.GetContainer(Synonym).Returns(container);

        //Act
        _service.InsertWord(NewWord, Synonym);

        //Assert
        _synonymData.Received(1).ContainsWord(NewWord);
        _synonymData.Received(1).GetContainer(Synonym);
        _synonymData.Received(1).AddContainer(Arg.Any<WordContainer>());
    }
    #endregion

    #region GetAllWords Tests

    [TestCase]
    public void GetAllWords_ShouldGetWordsFromSynonymData()
    {
        //Arrange
        ICollection<WordModel> words = new List<WordModel>()
        {
            new()
            {
                Value = NewWord,
                Id = Guid.NewGuid()
            }
        };
        _synonymData.GetAllWords().Returns(words);

        //Act
        var res = _service.GetAllWords();

        //Assert
        Assert.That(res, Is.EquivalentTo(words));
        _synonymData.Received(1).GetAllWords();
    }

    #endregion
}
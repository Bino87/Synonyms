using NSubstitute;
using Services.Interfaces;
using Services.Repositories;
using Services.Services;
using Shared.ErrorCodes;
using Shared.Models;

namespace Services.Tests;

public class SynonymRepositoryTests
{
    private const string Synonym = "synonym";
    private const string NewWord = "new word";
    private const int Id = 42;

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
        var synonymIds = new HashSet<int> {Id};
        var word = new Word()
        {
            Id = Id,
            Value = NewWord
        };
        WordContainer container = new WordContainer()
        {
            Word = word,
            SynonymmsId = synonymIds
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
        var synonymIds = new HashSet<int>(){Id, 2};
        var word = new Word()
        {
            Id = 1,
            Value = NewWord
        };
        WordContainer container = new WordContainer()
        {
            Word = word,
            SynonymmsId = synonymIds
        };
        _synonymData.GetWordsByIds(synonymIds).Returns(new[] { word, new Word(){Id = 2, Value = Synonym} });
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
    public void When_SynonymData_TriesToAddaWordWithVoutSynonym_NewCollectionOfSynonymsIsCreated_AndNoAttemptIsMadeToRetrieveSynonymCollection()
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
    public void When_SynonymData_TriesToAddaWordWithValidSynonym_CollecionOfThoseSynonymsIsTransferedToThatNewWord()
    {
        //Arrange

        WordContainer container = new WordContainer();
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
        ICollection<Word> words = new List<Word>()
        {
            new Word()
            {
                Value = NewWord,
                Id = 1
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
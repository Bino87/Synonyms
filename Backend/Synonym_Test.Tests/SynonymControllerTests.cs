using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Services.Interfaces;
using Shared.Dto;
using Shared.Models;
using Synonyms_Test.Controllers;

namespace Synonym_Test.Tests;

public class SynonymControllerTests
{
    private const string Word = "Word";
    private const string Synonym = "Synonym";
    private const string NewWord = "New Word";
    private const string ApiController = "api/[controller]";
    private const string GetSynonyms = "GetSynonyms";
    private const string AddNewWord = "AddNewWord";
    private const string GetAllWords = "GetAllWords";
    private SynonymController _controller = null!;
    private ISynonymService _synonymService = null!;
    private IMapper _mapper = null!;

    [SetUp]
    public void Setup()
    {
        _synonymService = Substitute.For<ISynonymService>();
        _mapper = Substitute.For<IMapper>();

        _controller = new SynonymController(_synonymService, _mapper);
    }

    [TestCase]
    public void ClassAttributeTest()
    {
        //Arrange
        var type = typeof(SynonymController);

        var apiControllerAttribute = type.GetCustomAttribute<ApiControllerAttribute>();
        var routeAttribute = type.GetCustomAttribute<RouteAttribute>();
        //Assert
        Assert.That(apiControllerAttribute, Is.Not.Null);
        Assert.That(routeAttribute, Is.Not.Null);
        Assert.That(routeAttribute?.Template, Is.EqualTo(ApiController));
    }

    #region GetSynonyms Tests
    [TestCase]
    public void GetSynonyms_AttributeTest()
    {
        //Arrange
        var type = typeof(SynonymController);
        var getSynonymsMethod = type.GetMethod(nameof(SynonymController.GetSynonyms));
        var getSynonymsMethodAttribute = getSynonymsMethod?.GetCustomAttribute<HttpPostAttribute>();

        //Assert
        Assert.That(getSynonymsMethod, Is.Not.Null);
        Assert.That(getSynonymsMethodAttribute, Is.Not.Null);
        Assert.That(getSynonymsMethodAttribute?.Template, Is.EqualTo(GetSynonyms));
    }

    [TestCase]
    public void GetSynonyms_Test()
    {
        //Arrange
        ICollection<WordModel> synonyms = new List<WordModel>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Value = Synonym
            }
        };
        ICollection<GetSynonymsResponseDto> mappedSynonyms = new List<GetSynonymsResponseDto>()
        {
            new()
            {
                Value = Synonym
            }
        };
        _synonymService.GetSynonyms(Word)
            .Returns(synonyms);
        _mapper.Map<ICollection<GetSynonymsResponseDto>>(synonyms)
            .Returns(mappedSynonyms);
        

        //Act
        var res = _controller.GetSynonyms(new GetSynonymsDto()
        {
            Value = Word
        });

        //Assert

        Assert.That(res, Is.Not.Null);
        Assert.That(res, Is.InstanceOf(typeof(ActionResult<ICollection<GetSynonymsResponseDto>>)));

        Assert.That(res.Result, Is.Not.Null);
        Assert.That(res.Result, Is.TypeOf<OkObjectResult>());

        var result = res.Result as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Value, Is.InstanceOf<ICollection<GetSynonymsResponseDto>>());


        var valueResponse = result?.Value as ICollection<GetSynonymsResponseDto>;
        Assert.That(valueResponse, Is.Not.Null);

        _synonymService.Received(1).GetSynonyms(Word);
        _mapper.Received(1).Map<ICollection<GetSynonymsResponseDto>>(synonyms);
    }

    [TestCase]
    public void When_ValueIsEmpty_ReturnsBadRequest()
    {
        //Act
        var result = _controller.GetSynonyms(new GetSynonymsDto()
        {
            Value = string.Empty
        });

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf(typeof(ActionResult<ICollection<GetSynonymsResponseDto>>)));
        Assert.That(result.Result, Is.Not.Null);
        Assert.That(result.Result, Is.TypeOf<BadRequestResult>());

    }

    [TestCase]
    public void When_ValueIsNull_ReturnsBadRequest()
    {
        //Act
        var result = _controller.GetSynonyms(new GetSynonymsDto()
        {
            Value = string.Empty
        });

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf(typeof(ActionResult<ICollection<GetSynonymsResponseDto>>)));
        Assert.That(result.Result, Is.Not.Null);
        Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
    }

    #endregion

    #region AddNewWord Tests

    [TestCase]
    public void AddNewWord_AttributeTest()
    {
        //Arrange
        var type = typeof(SynonymController);

        var addSynonymMethod = type.GetMethod(nameof(SynonymController.AddNewWord));

        var addSynonymMethodAttribute = addSynonymMethod?.GetCustomAttribute<HttpPostAttribute>();
        //Assert
        Assert.That(addSynonymMethod, Is.Not.Null);
        Assert.That(addSynonymMethodAttribute, Is.Not.Null);
        Assert.That(addSynonymMethodAttribute?.Template, Is.EqualTo(AddNewWord));
    }

    [TestCase]
    public void When_NewWordIsEmpty_ReturnsBadRequest()
    {
        //Act
        var result = _controller.AddNewWord(new AddSynonymRequestDto()
        {
            Synonym = Synonym,
            NewWord = string.Empty
        });

        //Assert
        Assert.That(result, Is.TypeOf<BadRequestResult>());
    }

    [TestCase]
    public void When_NewWordIsNull_ReturnsBadRequest()
    {
        //Act
        var result = _controller.AddNewWord(new AddSynonymRequestDto()
        {
            Synonym = Synonym,
            NewWord = string.Empty
        });

        //Assert
        Assert.That(result, Is.TypeOf<BadRequestResult>());

    }

    [TestCase]
    public void When_NewWordIsValid_ReturnsOkRequest()
    {
        //Arrange

        //Act
        var res = _controller.AddNewWord(new AddSynonymRequestDto()
        {
            Synonym = Synonym,
            NewWord = NewWord
        });
        //Assert

        Assert.That(res, Is.Not.Null);
        Assert.That(res, Is.InstanceOf(typeof(ActionResult)));

        _synonymService.Received(1).AddWordWithSynonym(NewWord, Synonym);
    }

    #endregion

    #region GetllAllWords Tests

    [TestCase]
    public void GetAllWords_AttributeTest()
    {
        //Arrange
        var type = typeof(SynonymController);

        var getAllWordsMethod = type.GetMethod(nameof(SynonymController.GetAllWords));

        var getAllWordsAttribute = getAllWordsMethod?.GetCustomAttribute<HttpGetAttribute>();
        //Assert
        Assert.That(getAllWordsMethod, Is.Not.Null);
        Assert.That(getAllWordsAttribute, Is.Not.Null);
        Assert.That(getAllWordsAttribute?.Template, Is.EqualTo(GetAllWords));
    }

    [TestCase]
    public void GetAllWords_FlowTest()
    {
        //Arrange
        ICollection<WordModel> words = new List<WordModel>()
        {
            new()
            {
                Id = Guid.NewGuid(), Value = "b"
            },
            new()
            {
                Id = Guid.NewGuid(), Value = "a"
            },
        };
        ICollection<GetAllWordsResponseDto> getAllWordsResponseDtoCollection = new List<GetAllWordsResponseDto>()
        {
            new()
            {
                Value = "a"
            },
            new()
            {
                Value = "b"
            },
        };
        _synonymService.GetAllWords().Returns(words);
        _mapper.Map<ICollection<GetAllWordsResponseDto>>(Arg.Is<IEnumerable<WordModel>>(x => x.First().Value == "a" && x.Last().Value == "b"))
            .Returns(getAllWordsResponseDtoCollection);
        //Act
        var res = _controller.GetAllWords();

        //Assert
        _synonymService.Received(1)
            .GetAllWords();
        _mapper.Received(1)
            .Map<ICollection<GetAllWordsResponseDto>>(Arg.Is<IEnumerable<WordModel>>(x => x.First().Value == "a" && x.Last().Value == "b"));

    }

    #endregion

}
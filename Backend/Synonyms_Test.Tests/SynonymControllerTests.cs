using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Synonym_Test.DataTransferObjects;
using Synonym_Test.Models;
using Synonyms_Test.Controllers;
using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.Services.Interfaces;

namespace Synonyms_Test.Tests;

public class SynonymControllerTests
{
    private const int SynonymId = 1;
    private const string Word = "word";
    private SynonymController _controller;
    private IMapper _mapper;
    private ISynonymService _synonymService;
    private IResponseFactory _responseFactory;
    private IEndPointLoggerFactory _endPointLoggerFactory;



    [SetUp]
    public void Setup()
    {
        _mapper = Substitute.For<IMapper>();
        _synonymService = Substitute.For<ISynonymService>();
        _responseFactory = Substitute.For<IResponseFactory>();
        _endPointLoggerFactory = Substitute.For<IEndPointLoggerFactory>();

        _controller = new SynonymController(_mapper, _synonymService, _responseFactory, _endPointLoggerFactory);
    }

    #region AddNewWordAsync Tests

    [TestCase]
    public void AddNewWordAsync_ShouldHave_HttpPostAttribute()
    {
        //Act
        var res = _controller.GetType().GetMethod(nameof(_controller.AddNewWordAsync)).GetCustomAttribute<HttpPostAttribute>();

        //Assert
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Template, Is.EqualTo("AddNewWord"));
    }

    [TestCase]
    public async Task AddNewWordAsync_WithOnlyWord()
    {
        //Arrange
        AddNewWordRequestDto dto = new AddNewWordRequestDto()
        {
            Value = Word
        };

        //Act
        var res = await _controller.AddNewWordAsync(dto);

        //Assert

        _endPointLoggerFactory.Received(1).CreateEndPointLogger(nameof(_controller.AddNewWordAsync));
        await _synonymService.Received(1).AddWordAsync(Word, null);
        _responseFactory.Received(1).CreateResponse();

    }

    [TestCase]
    public async Task AddNewWordAsync_WithWordAndSynonym()
    {
        //Arrange
        AddNewWordRequestDto dto = new AddNewWordRequestDto()
        {
            Value = Word,
            SynonymId = SynonymId
        };

        //Act
        var res = await _controller.AddNewWordAsync(dto);

        //Assert

        _endPointLoggerFactory.Received(1).CreateEndPointLogger(nameof(_controller.AddNewWordAsync));
        await _synonymService.Received(1).AddWordAsync(Word, SynonymId);
        _responseFactory.Received(1).CreateResponse();
    }

    #endregion

    #region GetAllWordsAsync Tests

    [TestCase]
    public void GetAllWordsAsync_ShouldHave_HttpGetAttribute()
    {
        //Act
        var res = _controller.GetType().GetMethod(nameof(_controller.GetAllWordsAsync)).GetCustomAttribute<HttpGetAttribute>();

        //Assert
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Template, Is.EqualTo("GetAllWords"));
    }

    [TestCase]
    public async Task GetAllWordsAsync_FlowTest()
    {
        //Arrange
        ICollection<Word> words = new List<Word>()
        {
            new Word()
            {
                Value = Word,
                WordId = SynonymId
            }
        };

        ICollection<GetAllWordsResponseDto> mappedResults = new List<GetAllWordsResponseDto>()
        {
            new()
            {
                Value = Word,
                Id = SynonymId
            }
        };

        _synonymService.GetAllWordsAsync().Returns(Task.FromResult(words));
        _mapper.Map<ICollection<GetAllWordsResponseDto>>(words).Returns(mappedResults);

        //Act
        var res = await _controller.GetAllWordsAsync();

        //Assert
        _endPointLoggerFactory.Received(1).CreateEndPointLogger(nameof(_controller.GetAllWordsAsync));
        await _synonymService.Received(1).GetAllWordsAsync();
        _responseFactory.Received(1).CreateResponse(mappedResults);

    }

    #endregion

    #region GetSynonymsAsync Tests

    [TestCase]
    public void GetSynonymsAsync_ShouldHave_HttpPostAttribute()
    {
        //Act
        var res = _controller.GetType().GetMethod(nameof(_controller.GetSynonymsAsync)).GetCustomAttribute<HttpPostAttribute>();

        //Assert
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Template, Is.EqualTo("GetSynonyms"));
    }

    [TestCase]
    public async Task GetSynonymsAsync_FlowTest()
    {
        //Arrange
        GetSynonymsRequestDto dto = new GetSynonymsRequestDto()
        {
            Value = Word
        };
        ICollection<GraphSearchResults> words = new List<GraphSearchResults>()
        {
            new(0,"wordOne"),
            new(1,"wordTwo"),
        };
        ICollection<GetSynonymsResponseDto> mappedResults = new List<GetSynonymsResponseDto>()
        {
            new() { Value = "wordOne", Closeness = 0},
            new() { Value = "wordTwo",Closeness = 1},
        };

        _synonymService.GetSynonymsAsync(Word).Returns(Task.FromResult(words));
        _mapper.Map<ICollection<GetSynonymsResponseDto>>(words).Returns(mappedResults);

        //Act
        var res = await _controller.GetSynonymsAsync(dto);

        //Assert
        _endPointLoggerFactory.Received(1).CreateEndPointLogger(nameof(_controller.GetSynonymsAsync));
        await _synonymService.Received(1).GetSynonymsAsync(Word);
        _mapper.Received(1).Map<ICollection<GetSynonymsResponseDto>>(words);
        _responseFactory.Received(1).CreateResponse(mappedResults);

    }

    #endregion

    #region Setup Tests

    [TestCase]
    public void SynonymController_ShouldHave_ApiControllerAttribute()
    {
        //Arrange
        var res = _controller.GetType().GetCustomAttribute<ApiControllerAttribute>();

        //Assert
        Assert.That(res, Is.Not.Null);
    }

    [TestCase]
    public void SynonymController_ShouldHave_RouteAttribute()
    {
        //Arrange
        var res = _controller.GetType().GetCustomAttribute<RouteAttribute>();

        //Assert
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Template, Is.EqualTo("api/[controller]"));
    }

    [TestCase]
    public void SynonymController_ShouldHave_AuthorizeAttribute()
    {
        //Arrange
        var res = _controller.GetType().GetCustomAttribute<AuthorizeAttribute>();

        //Assert
        Assert.That(res, Is.Not.Null);
        Assert.That(res.AuthenticationSchemes, Is.EqualTo("BasicAuthentication"));
    }

    #endregion

}
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Services.Interfaces;
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

    #endregion

}
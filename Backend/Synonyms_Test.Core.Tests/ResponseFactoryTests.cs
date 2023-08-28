using NSubstitute;
using Synonyms_Test.Core.Implementation;
using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.Core.Responses;

namespace Synonyms_Test.Core.Tests;

public class ResponseFactoryTests
{

    private IResponseFactory _service;
    private IResponseErrorHandler _responseErrorHandler;

    [SetUp]
    public void Setup()
    {
        _responseErrorHandler = Substitute.For<IResponseErrorHandler>();
        _service = new ResponseFactory(_responseErrorHandler);

        _responseErrorHandler.GetErrorCodes().Returns(Enumerable.Range(0, 3).ToArray());
    }

    [TestCase]
    public void CreateResponse_CreateResponseWithoutValueTest()
    {
        //Act
        var res = _service.CreateResponse();

        Assert.That(res, Is.Not.Null);
        Assert.IsInstanceOf<Response>(res);
        Assert.That(res.ErrorCodes.Length, Is.EqualTo(3));
        Assert.That(res.ErrorCodes[0], Is.EqualTo(0));
        Assert.That(res.ErrorCodes[1], Is.EqualTo(1));
        Assert.That(res.ErrorCodes[2], Is.EqualTo(2));
    }

    [TestCase]
    public void CreateResponse_CreateResponseWithValueTest()
    {
        //Arrange 
        object value = 123;
        //Act
        var res = _service.CreateResponse(value);

        Assert.That(res, Is.Not.Null);
        Assert.IsInstanceOf<Response>(res);
        Assert.That(res.ErrorCodes.Length, Is.EqualTo(3));
        Assert.That(res.ErrorCodes[0], Is.EqualTo(0));
        Assert.That(res.ErrorCodes[1], Is.EqualTo(1));
        Assert.That(res.ErrorCodes[2], Is.EqualTo(2));
        Assert.IsAssignableFrom<ValueResponse<object>>(res);
        ValueResponse<object> actual = (ValueResponse<object>)res;
        Assert.That(actual.Response, Is.EqualTo(value));
    }

}
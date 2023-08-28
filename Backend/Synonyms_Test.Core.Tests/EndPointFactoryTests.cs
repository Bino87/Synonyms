using Microsoft.Extensions.Logging;
using NSubstitute;
using Synonyms_Test.Core.Implementation;
using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.Core.Logging;

namespace Synonyms_Test.Core.Tests;

public class EndPointFactoryTests
{
    private IEndPointLoggerFactory _service;
    private ILogger<EndPointLoggerFactory> _logger;
    private IResponseErrorHandler _responseErrorHandler;
    private ITimeHandler _timeHandler;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<EndPointLoggerFactory>>();
        _responseErrorHandler = Substitute.For<IResponseErrorHandler>();
        _timeHandler = Substitute.For<ITimeHandler>();
        _service = new EndPointLoggerFactory(_logger, _responseErrorHandler,_timeHandler);
    }

    [Test]
    public void CreateEndPointLogger_And_DisposeFlowTest()
    {
        //Arrange
        _timeHandler.GetUtcNow().Returns(new DateTime(1000, 1, 1, 1, 1, 0));
        //Act
        var res = _service.CreateEndPointLogger("METHOD");
        //Assert

        Assert.IsNotNull(res);
        Assert.IsInstanceOf<EndPointLogger>(res);
        _timeHandler.Received(1).GetUtcNow();
        _timeHandler.GetUtcNow().Returns(new DateTime(1000, 1, 1, 1, 1, 1));

        res.Dispose();
        _timeHandler.Received(2).GetUtcNow();
        _responseErrorHandler.Received(1).GetErrorCodes();
        _logger.Received(1).Log(LogLevel.Information, 0, "Call to 'METHOD' was completed and took : 1000 ms with No Error Codes");
          
    }
}
using Microsoft.Extensions.Logging;
using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.Core.Logging;

namespace Synonyms_Test.Core.Implementation;

internal sealed class EndPointLoggerFactory : IEndPointLoggerFactory
{
    private readonly ILogger<EndPointLoggerFactory> _logger;
    private readonly IResponseErrorHandler _responseErrorHandler;
    private readonly ITimeHandler _timeHandler;

    public EndPointLoggerFactory(ILogger<EndPointLoggerFactory> logger, IResponseErrorHandler responseErrorHandler, ITimeHandler timeHandler)
    {
        _logger = logger;
        _responseErrorHandler = responseErrorHandler;
        _timeHandler = timeHandler;
    }

    public EndPointLogger CreateEndPointLogger(string method)
    {
        //Create endpoint logger, it will log endpoint data once disposed of.
        return new EndPointLogger(_logger, _responseErrorHandler,_timeHandler, method);
    }
}
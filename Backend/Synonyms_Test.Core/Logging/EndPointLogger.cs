using Microsoft.Extensions.Logging;
using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Logging
{
    public class EndPointLogger : IDisposable
    {
        private const string ErrorCodesMessage = "No Error Codes";
        private const string Separator = ", ";
        private readonly ILogger _logger;
        private readonly IResponseErrorHandler _responseErrorHandler;
        private readonly ITimeHandler _timeHandler;
        private readonly string _method;
        private readonly DateTime _timeStamp;

        internal EndPointLogger(ILogger logger, IResponseErrorHandler responseErrorHandler, ITimeHandler timeHandler, string method)
        {
            _logger = logger;
            _responseErrorHandler = responseErrorHandler;
            _timeHandler = timeHandler;
            _method = method;
            _timeStamp = _timeHandler.GetUtcNow();
        }

        public void Dispose()
        {
            var timeStamp = _timeHandler.GetUtcNow();
            var time = timeStamp - _timeStamp;
            var errorCodes = _responseErrorHandler.GetErrorCodes();
            var errorCodesMessage = errorCodes.Any() ? $"Error codes: {string.Join(Separator, errorCodes)}" : ErrorCodesMessage;
            _logger.LogInformation($"Call to '{_method}' was completed and took : {time.TotalMilliseconds} ms with {errorCodesMessage}");
        }
    }
}

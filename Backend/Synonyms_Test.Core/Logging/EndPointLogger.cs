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
        private readonly string _method;
        private readonly DateTime _timeStamp;

        internal EndPointLogger(ILogger logger, IResponseErrorHandler responseErrorHandler, string method)
        {
            _logger = logger;
            _responseErrorHandler = responseErrorHandler;
            _method = method;
            _timeStamp = DateTime.UtcNow;
        }

        public void Dispose()
        {
            var timeStamp = DateTime.UtcNow;
            var time = timeStamp - _timeStamp;
            var errorCodes = _responseErrorHandler.GetErrorCodes();
            var errorCodesMessage = errorCodes.Any() ? $"Error codes: {string.Join(Separator, errorCodes)}" : ErrorCodesMessage;
            _logger.LogInformation($"Call to '{_method}' was completed and took : {time.TotalMilliseconds} ms with {errorCodesMessage}");
        }
    }
}

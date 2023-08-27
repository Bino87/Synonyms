using Synonyms_Test.Core.Logging;

namespace Synonyms_Test.Core.Interfaces;

public interface IEndPointLoggerFactory
{
    EndPointLogger CreateEndPointLogger(string method);
}
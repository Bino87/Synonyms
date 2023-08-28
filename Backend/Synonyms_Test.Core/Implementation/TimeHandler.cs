using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Implementation;

public sealed class TimeHandler : ITimeHandler
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }
}
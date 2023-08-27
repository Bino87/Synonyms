using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Implementation;

internal sealed class Settings : ISettings
{
    public int GetSearchDepthLimit()
    {
        //returns the maximum depth of graph search
        return 3;
    }
}
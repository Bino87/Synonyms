using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Implementation;

internal sealed class WordAdapter : IWordAdapter
{
    public string? Adapt(string? word)
    {
        //trim and force word to be lower case.
        return word?.Trim()
            .ToLower();
    }
}
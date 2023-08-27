using System.Diagnostics;

namespace Synonym_Test.Models;

[DebuggerDisplay("{Value}")]
public sealed class Word
{
    public int WordId { get; set; }
    public string? Value { get; set; }
    public HashSet<Word>? Synonyms { get; set; } = new();

}
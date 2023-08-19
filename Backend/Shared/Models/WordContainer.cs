namespace Shared.Models;

public sealed class WordContainer
{
    public WordModel WordModel { get; set; } = null!;
    public HashSet<Guid> SynonymIds { get; set; } = null!;
}
namespace Shared.Models;

public class WordContainer
{
    public WordModel WordModel { get; set; } = null!;
    public HashSet<Guid> SynonymIds { get; set; } = null!;
}
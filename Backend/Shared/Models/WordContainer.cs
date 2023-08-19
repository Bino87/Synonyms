namespace Shared.Models;

public class WordContainer
{
    public Word Word { get; set; } = null!;
    public HashSet<int> SynonymmsId { get; set; } = null!;
}
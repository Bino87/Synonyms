namespace Synonym_Test.Models;

public sealed class GraphSearchResults
{
    public int Depth { get; set; }
    public string Word { get; set; }

    public GraphSearchResults(int depth, string word)
    {
        Depth = depth;
        Word = word;
    }
}
using Shared.Models;

namespace Services.Interfaces;

public interface ISynonymData
{
    public int WordCount { get;  }
    ICollection<Word> GetWordsByIds(HashSet<int> synonymsId);
    bool ContainsWord(string word);
    void AddContainer(WordContainer newWordContainer);
    WordContainer? GetContainer(string word);
    ICollection<Word> GetAllWords();
}
using Shared.Models;

namespace Services.Interfaces;

public interface ISynonymData
{
    public int WordCount { get;  }
    ICollection<WordModel> GetWordsByIds(HashSet<Guid> synonymsId);
    bool ContainsWord(string word);
    void AddContainer(WordContainer container);
    WordContainer? GetContainer(string word);
    ICollection<WordModel> GetAllWords();
}
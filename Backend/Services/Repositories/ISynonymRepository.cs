using Shared.Models;

namespace Services.Repositories;

public interface ISynonymRepository
{
    ICollection<Word> GetSynonyms(string word);
    void InsertWord(string newWord, string? synonym);
    ICollection<Word> GetAllWords();
}
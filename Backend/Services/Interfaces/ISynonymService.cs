
using Shared.Models;

namespace Services.Interfaces;

public interface ISynonymService
{
    ICollection<Word> GetSynonyms(string word);
    void AddWordWithSynonym(string newWord, string? synonym);
    ICollection<Word> GetAllWords();
}
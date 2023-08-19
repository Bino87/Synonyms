
using Shared.Models;

namespace Services.Interfaces;

public interface ISynonymService
{
    ICollection<WordModel> GetSynonyms(string word);
    void AddWordWithSynonym(string newWord, string? synonym);
    ICollection<WordModel> GetAllWords();
}
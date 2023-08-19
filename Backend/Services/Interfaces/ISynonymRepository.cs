using Shared.Models;

namespace Services.Interfaces;

public interface ISynonymRepository
{
    ICollection<WordModel> GetSynonyms(string word);
    void InsertWord(string newWord, string? synonym);
    ICollection<WordModel> GetAllWords();
}
using Synonym_Test.Models;

namespace Synonyms_Test.DataAccess.Interfaces;

public interface ISynonymsRepository
{
    Task AddWordAsync(string? word, int? synonym);
    Task<ICollection<Word>> GetAllWordsAsync();

    Task<bool> HasWordAsync(string? value);
    Task<Word?> GetWordWithAllSynonymsByValueAsync(string? value, int depthLimit);
    Task<bool> HasWordWithIdAsync(int id);
}
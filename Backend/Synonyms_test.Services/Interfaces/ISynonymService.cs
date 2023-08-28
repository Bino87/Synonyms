using Synonym_Test.Models;

namespace Synonyms_Test.Services.Interfaces;

public interface ISynonymService
{
    Task AddWordAsync(string? word, int? synonym);
    Task<ICollection<Word>> GetAllWordsAsync();
    Task<ICollection<GraphSearchResults>> GetSynonymsAsync(string? value);
}
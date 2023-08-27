using Synonym_Test.Models;

namespace Synonyms_Test.Services.Interfaces;

public interface ISynonymService
{
    Task AddWord(string? word, int? synonym);
    Task<ICollection<Word>> GetAllWords();
    Task<ICollection<GraphSearchResults>> GetSynonymsAsync(string? value);
}
using Services.Interfaces;
using Services.Repositories;
using Shared.Models;

namespace Services.Services;

internal class SynonymService : ISynonymService
{
    private readonly ISynonymRepository _synonymRepository;

    public SynonymService(ISynonymRepository synonymRepository)
    {
        _synonymRepository = synonymRepository;
    }

    /// <summary>
    /// Gets synonyms of a word
    /// </summary>
    /// <param name="word">value of a word</param>
    /// <returns>collection of synonyms to a word</returns>
    public ICollection<Word> GetSynonyms(string word)
    {
        return _synonymRepository.GetSynonyms(word);
    }

    /// <summary>
    /// Adds a word to collection with a synonym
    /// </summary>
    /// <param name="newWord">value of a word</param>
    /// <param name="synonym">value of a synonym, can be null</param>
    public void AddWordWithSynonym(string newWord, string? synonym)
    {
        _synonymRepository.InsertWord(newWord, synonym);
    }

    public ICollection<Word> GetAllWords()
    {
        return _synonymRepository.GetAllWords();
    }
}
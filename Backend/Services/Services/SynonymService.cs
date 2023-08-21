using Services.Interfaces;
using Shared.Models;

namespace Services.Services;

internal sealed class SynonymService : ISynonymService
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
    public ICollection<WordModel> GetSynonyms(string word)
    {
        return _synonymRepository.GetSynonyms(NormalzieWords(word));
    }

    /// <summary>
    /// Adds a word to collection with a synonym
    /// </summary>
    /// <param name="newWord">value of a word</param>
    /// <param name="synonym">value of a synonym, can be null</param>
    public void AddWordWithSynonym(string newWord, string? synonym)
    {
        _synonymRepository.InsertWord(NormalzieWords(newWord), NormalzieWords(synonym));
    }

    public ICollection<WordModel> GetAllWords()
    {
        return _synonymRepository.GetAllWords();
    }

    private string NormalzieWords(string word)
    {
        return word.ToLowerInvariant();
    }
}
using Services.Interfaces;
using Shared.Models;

namespace Services.Repositories;

internal sealed class SynonymData : ISynonymData
{
    private readonly IList<WordModel> _words;
    private readonly IDictionary<string, WordContainer> _wordLookup;

    public int WordCount => _words.Count;

    public SynonymData()
    {
        _words = new List<WordModel>();
        _wordLookup = new Dictionary<string, WordContainer>();
    }

    /// <summary>
    /// Gets words by Id
    /// </summary>
    /// <param name="synonymsId"></param>
    /// <returns>collection of words</returns>
    public ICollection<WordModel> GetWordsByIds(HashSet<Guid> synonymsId)
    {
        return _words.Where(x => synonymsId.Contains(x.Id)).ToArray();
    }


    /// <summary>
    /// Checks if a word already exists in a dataset
    /// </summary>
    /// <param name="word"></param>
    /// <returns>returns true if value already exists.</returns>
    public bool ContainsWord(string word)
    {
        return _wordLookup.ContainsKey(word);
    }

    /// <summary>
    /// Adds a word container to dataset.
    /// </summary>
    /// <param name="container">Container to be added</param>
    public void AddContainer(WordContainer container)
    {
        container.SynonymIds.Add(container.WordModel.Id);
        _words.Add(container.WordModel);
        _wordLookup.Add(container.WordModel.Value, container);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public WordContainer? GetContainer(string word)
    {
        _wordLookup.TryGetValue(word, out var container);
        return container;
    }

    /// <summary>
    /// Get all words that are in the system
    /// </summary>
    /// <returns> collection of all words</returns>
    public ICollection<WordModel> GetAllWords()
    {
        return _words;
    }
}
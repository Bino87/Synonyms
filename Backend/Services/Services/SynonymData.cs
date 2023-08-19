using Services.Interfaces;
using Shared.Models;

namespace Services.Services;

internal sealed class SynonymData : ISynonymData
{
    private readonly IList<Word> _words;
    private readonly IDictionary<string, WordContainer> _wordLookup;

    public int WordCount => _words.Count;

    public SynonymData()
    {
        _words = new List<Word>();
        _wordLookup = new Dictionary<string, WordContainer>();
    }

    /// <summary>
    /// Gets words by Id
    /// </summary>
    /// <param name="synonymsId"></param>
    /// <returns>collection of words</returns>
    public ICollection<Word> GetWordsByIds(HashSet<int> synonymsId)
    {
        return _words.Where(x => synonymsId.Contains(x.Id)).ToArray();
    }


    /// <summary>
    /// Checks if a word already exists in a dataset
    /// </summary>
    /// <param name="word"value of the word></param>
    /// <returns>returns true if value already exists.</returns>
    public bool ContainsWord(string word)
    {
        return _wordLookup.ContainsKey(word);
    }

    /// <summary>
    /// Adds a word container to dataset.
    /// </summary>
    /// <param name="newWordContainer">Container to be added</param>
    public void AddContainer( WordContainer newWordContainer)
    {
        _words.Add(newWordContainer.Word);
        _wordLookup.Add(newWordContainer.Word.Value, newWordContainer);
    }

    public Word? GetWordByName(string name)
    {
        var container = GetContainer(name);

        return container?.Word;
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

    public ICollection<Word> GetAllWords()
    {
        return _words;
    }
}
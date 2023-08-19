using Services.Interfaces;
using Shared.Models;

namespace Services.Repositories;

internal sealed class SynonymRepository : ISynonymRepository
{

    private readonly ISynonymData _synonymData;

    public SynonymRepository(ISynonymData synonymData)
    {
        _synonymData = synonymData;
    }


    /// <summary>
    /// Returns all registered synonyms of a word
    /// </summary>
    /// <param name="word">value of the word</param>
    /// <returns>Collection os synonyms</returns>
    public ICollection<WordModel> GetSynonyms(string word)
    {
        //Get container of that word
        var container = _synonymData.GetContainer(word);

        //Check if container exists
        if (container is null)
        {
            //return empty array if it does not exist
            return Array.Empty<WordModel>();
        }

        //get all words that are synonyms 
        return _synonymData.GetWordsByIds(container.SynonymIds)
            //eliminate the word that was used to search, since it cannot be a synonym of itself
            .Where(x => x.Value != word)
            //Parse to array to ensure that it is of type ICollection instead of IEnumerable
            .ToArray();
    }

    /// <summary>
    /// Inserts new word to the data holder. Here in this example it is in memory storage. 
    /// </summary>
    /// <param name="newWord">Value of the new word</param>
    /// <param name="synonym">Synonym to that word</param>
    public void InsertWord(string newWord, string? synonym)
    {
        //Check if the words already exists
        if (_synonymData.ContainsWord(newWord))
        {
            return;
        }

       
        //Add new value to said container 
        AddToDataSet(newWord, synonym);
    }

    /// <summary>
    /// Returns the list of all words in the system
    /// </summary>
    /// <returns>returns the collection of all words in the system</returns>
    public ICollection<WordModel> GetAllWords()
    {
        return _synonymData.GetAllWords();
    }

    /// <summary>
    /// adds new word to dataset and places it in correct container, as well as updating related words so that they know it is related to them.
    /// </summary>
    /// <param name="newValue">new word that we're inserting</param>
    /// <param name="synonym">synonym of that word</param>
    private void AddToDataSet(string value, string? synonym)
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // +++++ Ideally the code below  would be handled by an Stored Procedure and held permanently in the database ++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //Create the word 
        var newValue = CreateWord(value);

        //Try get synonym container
        var container = string.IsNullOrWhiteSpace(synonym) ? null : _synonymData.GetContainer(synonym);

        //Create new word container for the new word
        var newWordContainer = new WordContainer()
        {
            WordModel = newValue,
            //check if the container exists, if it doesn't exists that means synonym value is not in the data set ( most likely null )
            SynonymIds = container?.SynonymIds ?? new HashSet<Guid>()
        };

        //add new word to data set
        _synonymData.AddContainer(newWordContainer);
    }

    /// <summary>
    /// Creates a new word
    /// </summary>
    /// <param name="value">value of a word</param>
    /// <returns>new Word with value</returns>
    private WordModel CreateWord(string value)
    {
        var newValue = new WordModel()
        {
            Id = Guid.NewGuid(),
            Value = value
        };
        return newValue;
    }
}
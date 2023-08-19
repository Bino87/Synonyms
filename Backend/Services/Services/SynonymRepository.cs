using Services.Interfaces;
using Services.Repositories;
using Shared.ErrorCodes;
using Shared.Models;

namespace Services.Services;

internal sealed class SynonymRepository : ISynonymRepository
{

    private readonly ISynonymData _synonymData;

    public SynonymRepository( ISynonymData synonymData)
    {
        _synonymData = synonymData;
    }


    /// <summary>
    /// Returns all registered synonyms of a word
    /// </summary>
    /// <param name="word">value of the word</param>
    /// <returns>Collection os synonyms</returns>
    public ICollection<Word> GetSynonyms(string word)
    {
        var container = _synonymData.GetContainer(word);

        if (container is null)
        {
            return Array.Empty<Word>();
        }

        return _synonymData.GetWordsByIds(container.SynonymmsId)
            .Where(x => x.Value != word)
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

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // +++++ Ideally the code below  would be handled by an Stored Procedure and held permanently in the database ++++
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //Create the word 
        var newValue = CreateWord(newWord);

        //Add new value to said container 
        AddToDataSet(newValue, synonym);
    }

    public ICollection<Word> GetAllWords()
    {
        return _synonymData.GetAllWords();
    }

    /// <summary>
    /// adds new word to dataset and places it in correct container, as well as updating related words so that they know it is related to them.
    /// </summary>
    /// <param name="newValue">new word that we're inserting</param>
    /// <param name="synonym">synonym of that word</param>
    /// <returns>Re</returns>
    private void AddToDataSet(Word newValue, string? synonym)
    {
        //Try get synonym container
        WordContainer? container = string.IsNullOrWhiteSpace(synonym) ? null : _synonymData.GetContainer(synonym);

        //Create new word container for the new word
        WordContainer newWordContainer = new WordContainer()
        {
            Word = newValue,
            //check if the container exists, if it doesn't exists that means synonym value is not in the data set ( most likely null )
            SynonymmsId = container?.SynonymmsId ?? new HashSet<int>()
        };

        newWordContainer.SynonymmsId.Add(newValue.Id);
        //add new word to data set
        _synonymData.AddContainer(newWordContainer);
    }

    private Word CreateWord(string newWord)
    {
        Word newValue = new Word()
        {
            Id = _synonymData.WordCount,
            Value = newWord
        };
        return newValue;
    }
}
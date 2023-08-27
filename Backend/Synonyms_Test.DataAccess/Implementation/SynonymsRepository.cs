using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Synonym_Test.Models;
using Synonyms_Test.Core.ErrorHandling;
using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.DataAccess.Context;
using Synonyms_Test.DataAccess.Interfaces;

namespace Synonyms_Test.DataAccess.Implementation;

internal sealed class SynonymsRepository : ISynonymsRepository
{
    private readonly IDbContextFactory<WordContext> _wordContextFactory;
    private readonly ILogger<SynonymsRepository> _logger;
    private readonly IResponseErrorHandler _responseErrorHandler;
    public SynonymsRepository(IDbContextFactory<WordContext> wordContextFactory, ILogger<SynonymsRepository> logger, IResponseErrorHandler responseErrorHandler)
    {
        _wordContextFactory = wordContextFactory;
        _logger = logger;
        _responseErrorHandler = responseErrorHandler;
    }

    private async Task<WordContext> GetContext()
    {
        return await _wordContextFactory.CreateDbContextAsync();
    }

    public async Task<ICollection<Word>> GetAllWordsAsync()
    {
        try
        {
            //get the context
            await using var context = await GetContext();

            //Get all of the words
            return await context.Words.ToListAsync();
        }
        catch (Exception exception)
        {
            //in the event of exception log the exception message, report error and return empty array.
            _logger.LogError(exception, exception.Message);
            _responseErrorHandler.SetErrorCode(ErrorCodes.UnableToRetrieveData);
            return Array.Empty<Word>();
        }
    }

    public async Task<bool> HasWordAsync(string? value)
    {
        try
        {
            //get the context
            await using var context = await GetContext();

            //check if the word exists
            return await context.Words.AnyAsync(x => x.Value!.Equals(value));
        }
        catch (Exception exception)
        {
            //in the event of exception log the exception message, report error and return empty array.
            _logger.LogError(exception, exception.Message);
            _responseErrorHandler.SetErrorCode(ErrorCodes.UnableToRetrieveData);
            return false;
        }
    }

    public async Task<Word?> GetWordWithAllSynonymsByValueAsync(string? value, int depthLimit)
    {
        try
        {
            //get the context
            await using var context = await GetContext();

            //Get the word with it's synonyms
            var word = await context.Words
                .Where(x => x.Value == value)
                .Include(x => x.Synonyms)
                .FirstOrDefaultAsync();

            //Check if the words was found
            if (word is not null)
            {
                //Load its synonyms.
                await LoadSynonymsRecursively(context,word.Synonyms, 1, depthLimit);
            }

            //return the word, its Synonyms property should be now populated with words
            return word;
        }
        catch (Exception exception)
        {
            //in the event of exception log the exception message, report error and return empty array.
            _logger.LogError(exception, exception.Message);
            _responseErrorHandler.SetErrorCode(ErrorCodes.UnableToRetrieveData);
            return null;
        }
    }

    public async Task<bool> HasWordWithIdAsync(int id)
    {
        try
        {
            //get the context
            await using var context = await GetContext();

            //Check if we have a word with id
            return await context.Words.AnyAsync(x => x.WordId == id);
        }
        catch (Exception exception)
        {
            //in the event of exception log the exception message, report error and return empty array.
            _logger.LogError(exception, exception.Message);
            _responseErrorHandler.SetErrorCode(ErrorCodes.UnableToRetrieveData);
            return false;
        }
    }

    private async Task LoadSynonymsRecursively(WordContext context, IEnumerable<Word>? words, int depth, int depthLimit)
    {
        //check if the synonyms exist
        if (words is null)
        {
            return;
        }

        //Check if we're too deep in the graph search
        if (depth >= depthLimit)
        {
            return;
        }

        //Load synonyms
        var synonymsToLoad = words
            .Where(w => context.Entry(w).Collection(x => x.Synonyms!).IsLoaded == false)
            .ToList();

        
        foreach (var synonym in synonymsToLoad)
        {
            // load the values of the synonyms ( synonyms of the word we're checking )
            await context.Entry(synonym).Collection(x => x.Synonyms!).LoadAsync();
            //go deeper into recursion and repeat the process.
            //Do not forget to increment the depth.
            await LoadSynonymsRecursively(context,synonym.Synonyms, depth + 1, depthLimit);
        }
    }

    public async Task AddWordAsync(string? word, int? synonym)
    {
        try
        {
            //get the context
            await using var context = await GetContext();

            //prepare the word model
            var newWord = new Word { Value = word, Synonyms = new HashSet<Word>() };

            //Check if we're adding a word with a synonym
            if (synonym.HasValue)
            {
                //get the synonym of the word
                var existingWord = await context.Words
                    .Include(w => w.Synonyms)
                    .FirstOrDefaultAsync(w => w.WordId == synonym.Value);

                //Check if the word was retrieved 
                if (existingWord is not  null)
                {
                    //initialize  the synonyms of that word if there are none (that means that the word never had any synonym assigned to it.)
                    existingWord.Synonyms ??= new HashSet<Word>();

                    //add the new word to the synonym
                    existingWord.Synonyms.Add(newWord);

                    //add the synonym to the new word
                    newWord.Synonyms.Add(existingWord);
                }
                else
                {
                    // if we're expecting a synonym, but it does not exist we should log an error and abort
                    _responseErrorHandler.SetErrorCode(ErrorCodes.SynonymDoesNotExists);
                    _logger.LogWarning($"Word with the id: '{synonym.Value}' was not retrieved!");
                    return;
                }
            }

            //add the new word to database
            await context.Words.AddAsync(newWord);

            //save the changes
            await context.SaveChangesAsync();

        }
        catch (Exception exception)
        {
            //in the event of exception log the exception message, report error and return empty array.
            _logger.LogError(exception, exception.Message);
           
        }
    }
}
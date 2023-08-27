using Microsoft.Extensions.Logging;
using Synonym_Test.Models;
using Synonyms_Test.Core.ErrorHandling;
using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.DataAccess.Interfaces;
using Synonyms_Test.Services.Interfaces;

namespace Synonyms_Test.Services.Implementation;

internal sealed class SynonymService : ISynonymService
{
    private readonly ISynonymsRepository _synonymsRepository;
    private readonly IWordAdapter _wordAdapter;
    private readonly ISettings _settings;
    private readonly IWordValidator _wordValidator;
    private readonly ILogger<SynonymService> _logger;
    private readonly IResponseErrorHandler _responseErrorHandler;

    public SynonymService(ISynonymsRepository synonymsRepository, IWordAdapter wordAdapter, ISettings settings, IWordValidator wordValidator, ILogger<SynonymService> logger, IResponseErrorHandler responseErrorHandler)
    {
        _synonymsRepository = synonymsRepository;
        _wordAdapter = wordAdapter;
        _settings = settings;
        _wordValidator = wordValidator;
        _logger = logger;
        _responseErrorHandler = responseErrorHandler;
    }

    public async Task AddWord(string? word, int? synonym)
    {
        //Normalize the string (make it lower case and trim white space on both ends)
        word = _wordAdapter.Adapt(word);

        //Check if it is a valid string only letters are allowed
        if (_wordValidator.Validate(word) is false)
        {
            return;
        }

        //Check if word already exists
        if (await _synonymsRepository.HasWordAsync(word))
        {
            //Word already exists, return it as an error
            _responseErrorHandler.SetErrorCode(ErrorCodes.WordAlreadyExists);
            return;
        }

        //Word is supposed to have a synonym, check if that synonym exists
        if (synonym.HasValue && await _synonymsRepository.HasWordWithIdAsync(synonym.Value) is false)
        {
            //Synonym word does not exists
            _responseErrorHandler.SetErrorCode(ErrorCodes.SynonymDoesNotExists);
           return;
        }

        //Add the word
        await _synonymsRepository.AddWordAsync(word, synonym);
    }

    public async Task<ICollection<Word>> GetAllWords()
    {
        //Get all words
        return await _synonymsRepository.GetAllWordsAsync();
    }

    public async Task<ICollection<GraphSearchResults>> GetSynonymsAsync(string? value)
    {
        //Normalize the string (make it lower case and trim white space on both ends)
        value = _wordAdapter.Adapt(value);

        //Check if it is a valid string only letters are allowed
        if (_wordValidator.Validate(value) is false)
        {
            return Array.Empty<GraphSearchResults>();
        }

        //Check if the word exists
        if (await _synonymsRepository.HasWordAsync(value) is false)
        {
            _responseErrorHandler.SetErrorCode(ErrorCodes.WordDoesNotExist);
            return Array.Empty<GraphSearchResults>();
        }

        //Get the word with all its synonyms
        var word = await _synonymsRepository.GetWordWithAllSynonymsByValueAsync(value, _settings.GetSearchDepthLimit());

        //Check if it was found, might be null duo to database exceptions and such
        if (word is null)
        {
            return Array.Empty<GraphSearchResults>();
        }

        //create set of already explored words, we do not want to check the same word multiple times. 
        var results = new HashSet<Word>()
        {
            word
        };

        //Look through the graph recursively.
        return AssignRecursively(results, word.Synonyms, 0)
            //Order the words based on the depth at which they were found
            .OrderBy(x => x.Depth)
            //parse to array, forces enumeration
            .ToArray();
    }

    private IEnumerable<GraphSearchResults> AssignRecursively(ISet<Word> results, IEnumerable<Word>? synonyms, int depth)
    {
        //abort if synonyms are null
        if (synonyms == null)
        {
            yield break;
        }

        //Check every synonym individually
        foreach (var word in synonyms)
        {
            //Check if the word has value assigned
            if (string.IsNullOrWhiteSpace(word.Value))
            {
                _logger.LogWarning($"Word with Id '{word.WordId}' was detected having no value!");
                continue;
            }

            //Check if that word was already looked at.
            if (results.Contains(word))
            {
                continue;
            }

            //We haven't checked this word yet, lets add it to the explored list before we look at it.
            results.Add(word);

            //return the word we look at including the graph depth
            yield return new GraphSearchResults(depth, word.Value);

            //Look at every synonym of that word recursively.
            foreach (var searchResult in AssignRecursively(results, word.Synonyms, depth + 1))
            {
                //return the results of each.
                yield return searchResult;
            }
        }
    }
}
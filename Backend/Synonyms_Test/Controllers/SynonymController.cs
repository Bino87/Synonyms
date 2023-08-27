using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Synonym_Test.DataTransferObjects;
using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.Core.Responses;
using Synonyms_Test.Services.Interfaces;

namespace Synonyms_Test.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "BasicAuthentication")]
public sealed class SynonymController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISynonymService _synonymService;
    private readonly IResponseFactory _responseFactory;
    private readonly ILogger<SynonymController> _logger;
    private readonly IEndPointLoggerFactory _endPointLoggerFactory;
    public SynonymController(IMapper mapper, ISynonymService synonymService, IResponseFactory responseFactory, ILogger<SynonymController> logger, IEndPointLoggerFactory endPointLoggerFactory)
    {
        _mapper = mapper;
        _synonymService = synonymService;
        _responseFactory = responseFactory;
        _logger = logger;
        _endPointLoggerFactory = endPointLoggerFactory;
    }

    [HttpPost("GetSynonyms")]
    public async Task<ActionResult<ValueResponse<ICollection<GetSynonymsResponseDto>>>> GetSynonymsAsync(GetSynonymsRequestDto dto)
    {
        //initialize the end point logger. it will log automatically when exiting the endpoint
        using var stuff = _endPointLoggerFactory.CreateEndPointLogger(nameof(GetSynonymsAsync));

        //get the results
        var results = await _synonymService.GetSynonymsAsync(dto.Value);

        //map the results to response dtos
        var mappedResults = _mapper.Map<ICollection<GetSynonymsResponseDto>>(results);

        //create the response
        return Ok(_responseFactory.CreateResponse(mappedResults));
    }

    [HttpPost("AddNewWord")]
    public async Task<ActionResult<Response>> AddNewWordAsync(AddNewWordRequestDto dto)
    {
        //initialize the end point logger. it will log automatically when exiting the endpoint
        using var stuff = _endPointLoggerFactory.CreateEndPointLogger(nameof(AddNewWordAsync));

        //Add the word
        await _synonymService.AddWord(dto.Value, dto.SynonymId);

        //create the response
        return Ok(_responseFactory.CreateResponse());
    }

    [HttpGet("GetAllWords")]
    public async Task<ActionResult<ValueResponse<ICollection<GetAllWordsResponseDto>>>> GetAllWordsAsync()
    {
        //initialize the end point logger. it will log automatically when exiting the endpoint
        using var stuff = _endPointLoggerFactory.CreateEndPointLogger(nameof(GetAllWordsAsync));

        //get the results
        var results = await _synonymService.GetAllWords();

        //map the results to response dtos
        var mappedResults = _mapper.Map<ICollection<GetAllWordsResponseDto>>(results);

        //create the response
        return Ok(_responseFactory.CreateResponse(mappedResults));
    }
}
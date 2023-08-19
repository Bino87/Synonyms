using System.Net.WebSockets;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dto;
using Shared.Models;

namespace Synonyms_Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SynonymController : ControllerBase
    {
        private readonly ILogger<SynonymController> _logger;
        private readonly ISynonymService _synonymService;
        private readonly IMapper _mapper;

        public SynonymController(ILogger<SynonymController> logger, ISynonymService synonymService, IMapper mapper)
        {
            _logger = logger;
            _synonymService = synonymService;
            _mapper = mapper;
        }

        [HttpPost("GetSynonyms")]
        public ActionResult<ICollection<GetSynonymsResponseDto>> GetSynonyms(GetSynonymsDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Value))
            {
                return BadRequest();
            }

            var synonyms = _synonymService.GetSynonyms(dto.Value);

            var result = _mapper.Map<ICollection<GetSynonymsResponseDto>>(synonyms);

            return  Ok(result);
        }

        [HttpPost("AddNewWord")]
        public ActionResult AddNewWord(AddSynonymRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.NewWord))
            {
                return BadRequest();
            }

            _synonymService.AddWordWithSynonym(requestDto.NewWord, requestDto.Synonym);

            return Ok();
        }

        [HttpGet("GetAllWords")]
        public ActionResult<ICollection<GetAllWordsResponseDto>> GetAllWords()
        {
            ICollection<Word> words = _synonymService.GetAllWords();
            
            var result = _mapper.Map<ICollection<GetAllWordsResponseDto>>(words.OrderBy(x => x.Value));
            return Ok(result);
        }
    }
}
using AutoMapper;
using Synonym_Test.DataTransferObjects;
using Synonym_Test.Models;

namespace Synonyms_Test.MapperProfiles;

public sealed class GetAllWordsProfile : Profile
{
    public GetAllWordsProfile()
    {
        CreateMap<Word, GetAllWordsResponseDto>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WordId));
    }
}
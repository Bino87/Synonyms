using AutoMapper;
using Synonym_Test.DataTransferObjects;
using Synonym_Test.Models;

namespace Synonyms_Test.MapperProfiles;

public sealed class GetSynonymsProfile : Profile
{
    public GetSynonymsProfile()
    {
        CreateMap<GraphSearchResults, GetSynonymsResponseDto>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Word))
            .ForMember(dest => dest.Closeness, opt => opt.MapFrom(src => src.Depth));
    }
}
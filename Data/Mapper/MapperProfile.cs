using Data.Dto;
using AutoMapper;
using Data.Models;

namespace Data.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<LanguageDto, Language>().ReverseMap();
            CreateMap<WordDto, Word>().ReverseMap();
            CreateMap<WordPropertyDto, WordProperty>().ReverseMap();

            CreateMap<DictionaryDto, Dictionary>().ReverseMap();
            CreateMap<EntryDto, Entry>().ReverseMap();
            CreateMap<MeaningDto, Meaning>().ReverseMap();

            CreateMap<ExampleDto, Example>().ReverseMap();
            CreateMap<FreeExpressionDto, FreeExpression>().ReverseMap();
        }
    }
}

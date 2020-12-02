using AutoMapper;
using Data.Dto;
using Data.Models;

namespace Data.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<LanguageDto, Language>().ReverseMap();
            CreateMap<CreateWord, Word>();
            CreateMap<UpdateWord, Word>();
            CreateMap<Word, GetWord>();
            CreateMap<WordPropertyDto, WordProperty>().ReverseMap();

            CreateMap<CreateDictionary, Dictionary>();
            CreateMap<Dictionary, GetDictionary>();
            CreateMap<CreateFreeExpression, FreeExpression>(); //UpdateFreeExpression inherits CreateFreeExpression
            CreateMap<FreeExpression, GetFreeExpression>();


            CreateMap<CreateEntry, Entry>(); //UpdateEntry inherits CreateEntry
            CreateMap<Entry, GetEntry>();
            CreateMap<CreateMeaning, Meaning>();
            CreateMap<UpdateMeaning, Meaning>();
            CreateMap<Meaning, GetMeaning>();

        }
    }
}

using Api.Dto;
using AutoMapper;
using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<LanguageDto, Language>().ReverseMap();
            CreateMap<WordDto, Word>().ReverseMap();

            CreateMap<DictionaryDto, Dictionary>().ReverseMap();
            CreateMap<ExpressionDto, Expression>().ReverseMap();
            CreateMap<EntryDto, Entry>().ReverseMap();
            CreateMap<MeaningDto, Meaning>().ReverseMap();

            CreateMap<SpeechPartDto, SpeechPart>().ReverseMap();
            CreateMap<WordPropertyDto, WordProperty>().ReverseMap();
        }
    }
}

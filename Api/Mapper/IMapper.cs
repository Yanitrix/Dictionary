using Api.Dto;
using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Mapper
{
    public interface IMapper
    {
        public Language Map(LanguageDto dto);

        public LanguageDto Map(Language language);

        public Word Map(WordDto dto);

        public WordDto Map(Word word);

        public SpeechPart Map(SpeechPartDto dto);

        public SpeechPartDto Map(SpeechPart speechPart);
    }
}

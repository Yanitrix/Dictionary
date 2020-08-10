using Api.Dto;
using Dictionary_MVC.Data;
using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Mapper
{
    public class Mapper : IMapper
    {
        public Language Map(LanguageDto dto)
        {
            var result = ValueMap(dto);
            foreach (var word in dto.Words)
            {
                result.Words.Add(ValueMap(word));
            }
            
            var list = new List<SpeechPart>();
            
            foreach (var sp in dto.SpeechParts)
            {
                list.Add(ShallowMap(sp));
            }

            result.SpeechParts = list;

            return result;
        }

        public LanguageDto Map(Language language)
        {
            var result = ShallowMap(language);
            foreach (var word in language.Words)
            {
                result.Words.Add(ShallowMap(word));
            }
            foreach (var sp in language.SpeechParts)
            {
                result.SpeechParts.Add(ShallowMap(sp));
            }

            return result;
        }

        public Word Map(WordDto dto)
        {
            throw new NotImplementedException();
        }

        public WordDto Map(Word word)
        {
            throw new NotImplementedException();
        }

        public SpeechPart Map(SpeechPartDto dto)
        {
            throw new NotImplementedException();
        }

        public SpeechPartDto Map(SpeechPart speechPart)
        {
            throw new NotImplementedException();
        }

        private Language ValueMap(LanguageDto dto)
        {
            return new Language
            {
                Name = dto.Name,
            };
        }

        private LanguageDto ShallowMap(Language language)
        {
            return new LanguageDto { Name = language.Name };
        }

        private Word ValueMap(WordDto dto)
        {
            return new Word
            {
                SourceLanguageName = dto.SourceLanguageName,
                SpeechPartName = dto.SpeechPartName,
                Value = dto.Value,

            };
        }

        private WordDto ShallowMap(Word word)
        {
            return new WordDto
            {
                SourceLanguageName = word.SourceLanguageName,
                SpeechPartName = word.SpeechPartName,
                Value = word.Value,
                ID = word.ID
            };
        }

        private SpeechPart ShallowMap(SpeechPartDto dto)
        {
            return new SpeechPart
            {
                LanguageName = dto.LanguageName,
                Name = dto.Name,

            };
        }

        private SpeechPartDto ShallowMap(SpeechPart obj)
        {
            return new SpeechPartDto
            {
                LanguageName = obj.LanguageName,
                Name = obj.Name,
            };
        }
    }
}

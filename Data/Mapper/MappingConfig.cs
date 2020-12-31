using Data.Dto;
using Data.Models;
using System.Linq;

namespace Data.Mapper
{
    public class MappingConfig : AbstractMappingConfiguration
    {
        public MappingConfig()
        {
            RegisterMapping<WordPropertyDto, WordProperty>(src =>
            {
                WordProperty dest = new();
                dest.Name = src.Name;
                dest.Values = new(src.Values.ToArray());
                return dest;
            });

            RegisterMapping<WordProperty, WordPropertyDto>(src =>
            {
                return new()
                {
                    Name = src.Name,
                    Values = src.Values,
                };
            });

            RegisterMapping<CreateWord, Word>(src =>
            {
                Word result = new();
                result.SourceLanguageName = src.SourceLanguageName;
                result.Value = src.Value;
                var func = ResolveMappingFunction<WordPropertyDto, WordProperty>();
                foreach (var i in src.Properties)
                    result.Properties.Add(func(i));

                return result;
            });

            RegisterMapping<UpdateWord, Word>(src =>
            {
                Word result = new();
                result.SourceLanguageName = src.SourceLanguageName;
                var func = ResolveMappingFunction<WordPropertyDto, WordProperty>();
                foreach (var i in src.Properties)
                    result.Properties.Add(func(i));

                return result;
            });

            RegisterMapping<Word, GetWord>(src =>
            {
                var func = ResolveMappingFunction<WordProperty, WordPropertyDto>();
                GetWord result = new();
                result.Value = src.Value;
                result.ID = src.ID;
                result.SourceLanguageName = src.SourceLanguageName;
                foreach (var i in src.Properties)
                    result.Properties.Add(func(i));
                return result;
            });

            RegisterMapping<CreateLanguage, Language>(src =>
            {
                return new()
                {
                    Name = src.Name
                };
            });

            RegisterMapping<Language, GetLanguage>(src =>
            {
                var func = ResolveMappingFunction<Word, GetWord>();
                var dest = new GetLanguage();
                dest.Name = src.Name;
                foreach (var i in src.Words)
                    dest.Words.Add(func(i));

                return dest;
            });

            RegisterMapping<CreateDictionary, Dictionary>(src =>
            {
                return new()
                {
                    LanguageInName = src.LanguageIn,
                    LanguageOutName = src.LanguageOut
                };
            });

            RegisterMapping<Dictionary, GetDictionary>(src =>
            {
                return new()
                {
                    Index = src.Index,
                    LanguageIn = src.LanguageInName,
                    LanguageOut = src.LanguageOutName
                };
            });

            RegisterMapping<CreateOrUpdateFreeExpression, FreeExpression>(src =>
            {
                return new()
                {
                    DictionaryIndex = src.DictionaryIndex,
                    Text = src.Text,
                    Translation = src.Translation
                };
            });

            RegisterMapping<FreeExpression, GetFreeExpression>(src =>
            {
                return new()
                {
                    DictionaryIndex = src.DictionaryIndex,
                    ID = src.ID,
                    Text = src.Text,
                    Translation = src.Translation
                };
            });

            RegisterMapping<CreateOrUpdateEntry, Entry>(src =>
            {
                return new()
                {
                    DictionaryIndex = src.DictionaryIndex,
                    WordID = src.WordID,
                };
            });

            RegisterMapping<Entry, GetEntry>(src =>
            {
                var meaningMap = ResolveMappingFunction<Meaning, GetMeaning>();
                var wordMap = ResolveMappingFunction<Word, GetWord>();
                var dicMap = ResolveMappingFunction<Dictionary, GetDictionary>();

                var dest = new GetEntry
                {
                    Dictionary = dicMap(src.Dictionary),
                    ID = src.ID,
                    Word = wordMap(src.Word)
                };
                foreach (var i in src.Meanings)
                    dest.Meanings.Add(meaningMap(i));

                return dest;
            });

            

            RegisterMapping<CreateMeaning, Meaning>(src =>
            {
                var func = ResolveMappingFunction<ExampleDto, Example>();
                var dest = new Meaning();
                dest.EntryID = src.EntryID;
                dest.Notes = src.Notes;
                dest.Value = src.Value;
                foreach (var i in src.Examples)
                    dest.Examples.Add(func(i));

                return dest;
            });

            RegisterMapping<UpdateMeaning, Meaning>(src =>
            {
                var func = ResolveMappingFunction<ExampleDto, Example>();
                var dest = new Meaning
                {
                    Notes = src.Notes,
                    Value = src.Value
                };
                foreach (var i in src.Examples)
                    dest.Examples.Add(func(i));

                return dest;
            });

            RegisterMapping<Meaning, GetMeaning>(src =>
            {
                var dest = new GetMeaning
                {
                    EntryID = src.EntryID,
                    ID = src.ID,
                    Notes = src.Notes,
                    Value = src.Value,
                };
                foreach (var i in src.Examples)
                    dest.Examples.Add(ResolveMappingFunction<Example, ExampleDto>()(i));

                return dest;
            });

            RegisterMapping<Example, ExampleDto>(src =>
            {
                return new()
                {
                    Text = src.Text,
                    Translation = src.Translation
                };
            });

            RegisterMapping<ExampleDto, Example>(src =>
            {
                return new()
                {
                    Text = src.Text,
                    Translation = src.Translation
                };
            });

        }
    }
}

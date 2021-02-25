using Domain.Dto;
using Domain.Models;
using System.Linq;

namespace Service.Mapper
{
    public class MappingConfig : AbstractMappingConfiguration
    {
        public MappingConfig()
        {
            RegisterMapping<WordPropertyDto, WordProperty>((src, dest) =>
            {
                dest.Name = src.Name;
                dest.Values = new(src.Values.ToArray());
                return dest;
            });

            RegisterMapping<WordProperty, WordPropertyDto>((src, dest) =>
            {
                dest.Name = src.Name;
                dest.Values = src.Values;
                return dest;
            });

            RegisterMapping<CreateWord, Word>((src, dest) =>
            {
                dest.SourceLanguageName = src.SourceLanguageName;
                dest.Value = src.Value;
                foreach (var i in src.Properties)
                    dest.Properties.Add(Map<WordPropertyDto, WordProperty>(i));

                return dest;
            });

            RegisterMapping<UpdateWord, Word>((src, dest) =>
            {
                dest.Value = src.Value;
                foreach (var i in src.Properties)
                    dest.Properties.Add(Map<WordPropertyDto, WordProperty>(i));

                return dest;
            });

            RegisterMapping<Word, GetWord>((src, dest) =>
            {
                dest.Value = src.Value;
                dest.ID = src.ID;
                dest.SourceLanguageName = src.SourceLanguageName;
                foreach (var i in src.Properties)
                    dest.Properties.Add(Map<WordProperty, WordPropertyDto>(i));
                return dest;
            });

            RegisterMapping<CreateLanguage, Language>((src, dest) =>
            {
                dest.Name = src.Name;
                return dest;
            });

            RegisterMapping<Language, GetLanguage>((src, dest) =>
            {
                dest.Name = src.Name;
                foreach (var i in src.Words)
                    dest.Words.Add(Map<Word, GetWord>(i));

                return dest;
            });

            RegisterMapping<CreateDictionary, Dictionary>((src, dest) =>
            {
                dest.LanguageInName = src.LanguageIn;
                dest.LanguageOutName = src.LanguageOut;
                return dest;
            });

            RegisterMapping<Dictionary, GetDictionary>((src, dest) =>
            {
                dest.Index = src.Index;
                dest.LanguageIn = src.LanguageInName;
                dest.LanguageOut = src.LanguageOutName;
                return dest;
            });

            RegisterMapping<CreateFreeExpression, FreeExpression>((src, dest) =>
            {
                dest.DictionaryIndex = src.DictionaryIndex;
                dest.Text = src.Text;
                dest.Translation = src.Translation;
                return dest;
            });

            RegisterMapping<UpdateFreeExpression, FreeExpression>((src, dest) =>
            {
                return new()
                {
                    DictionaryIndex = src.DictionaryIndex,
                    ID = src.ID,
                    Text = src.Text,
                    Translation = src.Translation
                };
            });

            RegisterMapping<FreeExpression, GetFreeExpression>((src, dest) =>
            {
                return new()
                {
                    DictionaryIndex = src.DictionaryIndex,
                    ID = src.ID,
                    Text = src.Text,
                    Translation = src.Translation
                };
            });

            RegisterMapping<CreateEntry, Entry>((src, dest) =>
            {
                return new()
                {
                    DictionaryIndex = src.DictionaryIndex,
                    WordID = src.WordID,
                };
            });

            RegisterMapping<UpdateEntry, Entry>((src, dest) =>
            {
                return new()
                {
                    DictionaryIndex = src.DictionaryIndex,
                    ID = src.ID,
                    WordID = src.WordID
                };
            });

            RegisterMapping<Entry, GetEntry>((src, dest) =>
            {
                dest.Dictionary = Map<Dictionary, GetDictionary>(src.Dictionary);
                dest.ID = src.ID;
                dest.Word = Map<Word, GetWord>(src.Word);
                foreach (var i in src.Meanings)
                    dest.Meanings.Add(Map<Meaning, GetMeaning>(i));

                return dest;
            });

            RegisterMapping<CreateMeaning, Meaning>((src, dest) =>
            {
                dest.EntryID = src.EntryID;
                dest.Notes = src.Notes;
                dest.Value = src.Value;
                foreach (var i in src.Examples)
                    dest.Examples.Add(Map<ExampleDto, Example>(i));

                return dest;
            });

            RegisterMapping<UpdateMeaning, Meaning>((src, dest) =>
            {
                dest.Notes = src.Notes;
                dest.Value = src.Value;
                foreach (var i in src.Examples)
                    dest.Examples.Add(Map<ExampleDto, Example>(i));

                return dest;
            });

            RegisterMapping<Meaning, GetMeaning>((src, dest) =>
            {
                dest.EntryID = src.EntryID;
                dest.ID = src.ID;
                dest.Notes = src.Notes;
                dest.Value = src.Value;
                foreach (var i in src.Examples)
                    dest.Examples.Add(Map<Example, ExampleDto>(i));

                return dest;
            });

            RegisterMapping<Example, ExampleDto>((src, dest) =>
            {
                dest.Text = src.Text;
                dest.Translation = src.Translation;
                return dest;
            });

            RegisterMapping<ExampleDto, Example>((src, dest) =>
            {
                dest.Text = src.Text;
                dest.Translation = src.Translation;
                return dest;
            });
        }
    }
}

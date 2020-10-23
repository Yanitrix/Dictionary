using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Tests
{
    public class DataInitializer
    {
        public List<Dictionary> Dictionaries { get; set; } = new List<Dictionary>();
        public List<Language> Languages { get; set; } = new List<Language>();

        public DataInitializer()
        {
            initialize();
        }

        public void initialize()
        {
            initializeLanguages();
            initializeWords();
            initializeDictionaries();
        }

        private void initializeDictionaries()
        {
            var en_de = new Dictionary
            {
                LanguageIn = Languages[0],
                LanguageInName = Languages[0].Name,
                LanguageOut = Languages[1],
                LanguageOutName = Languages[1].Name,
            };

            var de_en = new Dictionary
            {
                LanguageIn = Languages[1],
                LanguageInName = Languages[1].Name,
                LanguageOut = Languages[0],
                LanguageOutName = Languages[0].Name,
            };

            Dictionaries.Add(en_de);
            Dictionaries.Add(de_en);
        }

        private void initializeWords()
        {
            //2 nouns, 2 verbs

            var hund = new Word
            {
                ID = 1,
                SourceLanguage = Languages[0],
                SourceLanguageName = Languages[0].Name,
                Value = "hund",
                Properties = new WordPropertySet
                {
                    new WordProperty{Name = "declension", Values = new StringSet{"strong" }, ID = 1},
                    new WordProperty{Name = "gender", Values = new StringSet{"masculine" }, ID = 2}
                }
            };

            var essen = new Word
            {
                ID = 1,
                SourceLanguage = Languages[0],
                SourceLanguageName = Languages[0].Name,
                Value = "essen",
                Properties = new WordPropertySet
                {
                    new WordProperty { Name = "voice", Values = new StringSet{ "active" }, ID = 3 },
                    new WordProperty { Name = "conjugation", Values = new StringSet{"strong" }, ID = 4 }
                }
            };

            var dog = new Word
            {
                ID = 1,
                SourceLanguage = Languages[1],
                SourceLanguageName = Languages[1].Name,
                Value = "dog",
                Properties = new WordPropertySet
                {
                    new WordProperty { Name = "plural", Values = new StringSet{ "regular" }, ID = 5 },
                    new WordProperty { Name = "synonym", Values = new StringSet{ "humna's best friend" }, ID = 6 }
                }
            };

            var eat = new Word
            {
                ID = 1,
                SourceLanguage = Languages[1],
                SourceLanguageName = Languages[1].Name,
                Value = "eat",
                Properties = new WordPropertySet
                {
                    new WordProperty { Name = "conjugation", Values = new StringSet{"regular" }, ID = 7 },
                    new WordProperty { Name = "voice", Values = new StringSet{ "active" }, ID = 8 }
                }
            };

            Languages[0].Words.Add(hund);
            Languages[0].Words.Add(essen);
            Languages[1].Words.Add(dog);
            Languages[1].Words.Add(eat);
            
        }

        private void initializeLanguages()
        {
            Language german = new Language { Name = "german", };
            Language english = new Language { Name = "english", };

            Languages.Add(german);
            Languages.Add(english);
        }
    }
}


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
        public List<SpeechPart> SpeechParts { get; set; } = new List<SpeechPart>();

        public DataInitializer()
        {
            initialize();
        }

        public void initialize()
        {
            initializeSpeechParts();
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
                SpeechPartName = "noun",
                Value = "hund",
                Properties = new HashSet<WordProperty>
                {
                    new WordProperty{Name = "declension", Value = "strong", ID = 1},
                    new WordProperty{Name = "gender", Value = "masculine", ID = 2}
                }
            };

            var essen = new Word
            {
                ID = 1,
                SourceLanguage = Languages[0],
                SourceLanguageName = Languages[0].Name,
                SpeechPartName = "verb",
                Value = "essen",
                Properties = new HashSet<WordProperty>
                {
                    new WordProperty { Name = "voice", Value = "active", ID = 3 },
                    new WordProperty { Name = "conjugation", Value = "strong", ID = 4 }
                }
            };

            var dog = new Word
            {
                ID = 1,
                SourceLanguage = Languages[1],
                SourceLanguageName = Languages[1].Name,
                SpeechPartName = "noun",
                Value = "dog",
                Properties = new HashSet<WordProperty>
                {
                    new WordProperty { Name = "plural", Value = "regular", ID = 5 },
                    new WordProperty { Name = "synonym", Value = "humna's best friend", ID = 6 }
                }
            };

            var eat = new Word
            {
                ID = 1,
                SourceLanguage = Languages[1],
                SourceLanguageName = Languages[1].Name,
                SpeechPartName = "verb",
                Value = "eat",
                Properties = new HashSet<WordProperty>
                {
                    new WordProperty { Name = "conjugation", Value = "regular", ID = 7 },
                    new WordProperty { Name = "voice", Value = "active", ID = 8 }
                }
            };

            Languages[0].Words.Add(hund);
            Languages[0].Words.Add(essen);
            Languages[1].Words.Add(dog);
            Languages[1].Words.Add(eat);
            
        }

        private void initializeSpeechParts()
        {
            Language german = new Language { Name = "german", };
            Language english = new Language { Name = "english", };

            Languages.Add(german);
            Languages.Add(english);

            SpeechPart noun_de = new SpeechPart { Name = "noun", };
            SpeechPart verb_de = new SpeechPart { Name = "verb", };

            SpeechPartProperty verb_voice = new SpeechPartProperty
            {
                ID = 1,
                Name = "voice",
                PossibleValues = new HashSet<String>
                {
                    "active", "passive", "reflexive"
                },
                SpeechPart = verb_de
            };

            SpeechPartProperty verb_conjugation = new SpeechPartProperty
            {
                ID = 2,
                Name = "conjugation",
                PossibleValues = new HashSet<String>
                {
                    "weak", "strong", "mixed"
                },
                SpeechPart = verb_de
            };

            SpeechPartProperty noun_declension = new SpeechPartProperty
            {
                ID = 3,
                Name = "declension",
                PossibleValues = new HashSet<String>
                {
                    "weak", "strong", "mixed"
                },
                SpeechPart = noun_de
            };

            SpeechPartProperty noun_gender = new SpeechPartProperty
            {
                ID = 4,
                Name = "gender",
                PossibleValues = new HashSet<String>
                {
                    "masculine", "feminine", "neuter", "plural only"
                },
                SpeechPart = noun_de
            };

            noun_de.Properties = new List<SpeechPartProperty> { noun_declension, noun_gender };
            verb_de.Properties = new List<SpeechPartProperty> { verb_conjugation, verb_voice };


            SpeechPart noun_en = new SpeechPart { Name = "noun", };
            SpeechPart verb_en = new SpeechPart { Name = "verb", };

            SpeechPartProperty verb_voice_en = new SpeechPartProperty
            {
                ID = 5,
                Name = "voice",
                PossibleValues = new HashSet<String>
                {
                    "active", "passive"
                },
                SpeechPart = verb_en
            };

            SpeechPartProperty verb_conjugation_en = new SpeechPartProperty
            {
                ID = 6,
                Name = "conjugation",
                PossibleValues = new HashSet<String>
                {
                    "regular", "irregular"
                },
                SpeechPart = verb_en
            };

            SpeechPartProperty noun_antonym = new SpeechPartProperty
            {
                ID = 7,
                Name = "antonym",
                SpeechPart = noun_en
            };

            SpeechPartProperty noun_synonym = new SpeechPartProperty
            {
                ID = 8,
                Name = "synonym",
                SpeechPart = noun_en
            };

            SpeechPartProperty noun_plural = new SpeechPartProperty
            {
                ID = 9,
                Name = "plural",
                PossibleValues = new HashSet<String> { "regular", "irregular" },
                SpeechPart = noun_en
            };

            noun_en.Properties = new List<SpeechPartProperty> { noun_antonym, noun_synonym, noun_plural };
            verb_en.Properties = new List<SpeechPartProperty> { verb_conjugation_en, verb_voice_en };

            german.SpeechParts = new HashSet<SpeechPart> { verb_de, noun_de };
            english.SpeechParts = new HashSet<SpeechPart> { verb_en, noun_en };

            SpeechParts.AddRange(new List<SpeechPart> { verb_en, verb_de, noun_en, noun_de });

        }
    }
}


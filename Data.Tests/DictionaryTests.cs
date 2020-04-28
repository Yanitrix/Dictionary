using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Data.Tests
{
    public class DictionaryTests
    {
        public DictionaryTests()
        {
            //DataInitializer.initialize();
        }

        [Fact]
        public void testDictionary()
        {
            var data = new DataInitializer();

            var deToEn = data.Dictionaries.Where(x => x.LanguageInName == "german" && x.LanguageOutName == "english").Single();

            Assert.Equal("german", deToEn.LanguageInName);
            Assert.Equal("english", deToEn.LanguageOutName);
        }

        [Fact]
        public void testEntries()
        {
            var data = new DataInitializer();
            var deToEn = data.Dictionaries.Where(x => x.LanguageInName == "german" && x.LanguageOutName == "english").Single();

            deToEn.Entries = new HashSet<Entry>
            {
                new Entry
                {
                    ID = 1,
                    Dictionary = deToEn,
                    Word = data.Languages[0].Words.Single(w => w.Value == "hund"),
                    Meanings = new HashSet<Meaning>
                    {
                        new Meaning{Value = "dog"},
                        new Meaning{Value = "dumbass", Notes = "pejorative"}
                    }
                },
                new Entry
                {
                    ID = 2,
                    Dictionary = deToEn,
                    Word = data.Languages[0].Words.Single(w => w.Value == "essen"),
                    Meanings = new HashSet<Meaning>
                    {
                        new Meaning{Value = "eat"},
                        new Meaning{Value = "dine", Examples = new HashSet<Expression>
                            {
                                new Expression{Text = "von etw. essen", Translation = "to eat from sth."}
                            }
                        }
                    },
                    Expressions =  new HashSet<Expression>
                    {
                        new Expression{Text = "gegessen sein", Translation = "to be dead and buried"}
                    }
                }
            };

            var entry = deToEn.Entries.Single(e => e.Word.Value == "hund");
            var meanings = new List<Meaning>(entry.Meanings);

            Assert.Equal("dog", meanings[0].Value);
            Assert.Equal("dumbass", meanings[1].Value);
            Assert.Equal("pejorative", meanings[1].Notes);

            entry = deToEn.Entries.Single(e => e.Word.Value == "essen");
            meanings = new List<Meaning>(entry.Meanings);
            
            var example = meanings[1].Examples.Single(x => x.Equals(x));
            Assert.Equal("von etw. essen", example.Text);
            Assert.Equal("to eat from sth.", example.Translation);

            String str = deToEn.ToString();
            File.WriteAllText(@"C:\Users\domin\Desktop\output.txt", str);

            var expression = entry.Expressions.Single(x => x.Equals(x));
            Assert.Equal("gegessen sein", expression.Text);
            Assert.Equal("to be dead and buried", expression.Translation);

        }
    }
}
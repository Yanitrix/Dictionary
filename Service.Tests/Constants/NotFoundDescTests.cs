using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using msg = Commons.ValidationErrorMessages;

namespace Service.Tests.Constants
{
    public class NotFoundDescTests
    {
        [Fact]
        public void NotFoundDescWithEntry()
        {
            var entry = new Entry
            {
                DictionaryIndex = 12,
                WordID = 5
            };

            var dictMsg = $"Dictionary with given Index: {entry.DictionaryIndex} was not found in the database. Create it before posting a(n) Entry";
            var wordMsg = $"Word with given ID: {entry.WordID} was not found in the database. Create it before posting a(n) Entry";

            var actualDictMsg = msg.NOTFOUND_DESC<Entry, Dictionary>(d => d.Index, entry.DictionaryIndex);
            var actualWordMsg = msg.NOTFOUND_DESC<Entry, Word>(w => w.ID, entry.WordID);

            Assert.Equal(dictMsg, actualDictMsg);
            Assert.Equal(wordMsg, actualWordMsg);
        }

        [Fact]
        public void NotFoundWihtWord()
        {
            var word = new Word
            {
                SourceLanguageName = "posli",
            };

            var expectedMsg = $"Language with given Name: {word.SourceLanguageName} was not found in the database. Create it before posting a(n) Word";
            var actualMsg = msg.NOTFOUND_DESC<Word, Language>(l => l.Name, word.SourceLanguageName);

            Assert.Equal(expectedMsg, actualMsg);
        }
    }
}

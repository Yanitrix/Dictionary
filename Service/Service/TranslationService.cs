using Data.Models;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Service
{
    public class TranslationService : ITranslationService
    {
        private readonly IEntryRepository entries;
        private readonly IFreeExpressionRepository expressions;
        private readonly IDictionaryRepository dictionaries;
        private readonly IMeaningRepository meanings;
        private readonly IExampleRepository examples;

        public TranslationService(IUnitOfWork uow)
        {
            this.entries = uow.Entries;
            this.expressions = uow.FreeExpressions;
            this.dictionaries = uow.Dictionaries;
            this.meanings = uow.Meanings;
            this.examples = uow.Examples;
        }

        public bool EnsureDictionaryExists(String languageIn, String languageOut, bool bidirectional = false)
        {
            if (bidirectional)
            {
                return dictionaries.ExistsByLanguages(languageIn, languageOut)
                    || dictionaries.ExistsByLanguages(languageOut, languageIn);
            }

            return dictionaries.ExistsByLanguages(languageIn, languageOut);
        }

        // 1. Two separate lists for Entries (for this and that dictionary)
        // 2. If meanings are found, get their Entries and add to these lists
        // 3. FreeExpressions stay free (two separate lists for them)

        public IEnumerable<Entry> GetMatchingEntries(String languageIn, String languageOut, String query)
        {
            var dictionary = dictionaries.GetByLanguageInAndOut(languageIn, languageOut);
            var byWord = entries.GetByDictionaryAndWord(dictionary.Index, query, false); //case insensitive because we want all the results

            //TODO i think that method should be moved to EntryRepository because we want to get entries.
            var meaningsFound = meanings.GetByValueSubstring(query); //should be case insensitive
            if (meaningsFound.Any())
            {
                var ids = meaningsFound.Select(m => m.EntryID);
                var byMeaningValue = entries.Get(e => ids.Contains(e.ID), x => x);
                return byWord.Concat(byMeaningValue);
            }

            //searching also in examples? i think that if something is included in an Example for a Meaning, it should also be in the Meaning's value
            //i cannot think of an example that'd work the opposite way
            //so let's skip for now
            return byWord;
        }

        public (IEnumerable<Entry> unidirectional, IEnumerable<Entry> opposite) GetMatchingEntriesBidir(String languageIn, String languageOut, String query)
        {
            var unidicrectional = GetMatchingEntries(languageIn, languageOut, query);
            var opposite = GetMatchingEntries(languageOut, languageIn, query);

            return (unidicrectional, opposite);
            //TODO another thing - do we want to query by meanings' values in the opposite dictionary?
            //I don't think that's necessary but I'll play with the API and decide then
        }

        //then query free expressions
        public IEnumerable<FreeExpression> GetMatchingExpressions(String languageIn, String languageOut, String query)
        {
            var dictionary = dictionaries.GetByLanguageInAndOut(languageIn, languageOut);
            //we should return all FreeExpressions that contain the query both in Text and in Translation
            var byText = expressions.GetByDictionaryAndTextSubstring(dictionary.Index, query);
            var byTranslation = expressions.GetByDictionaryAndTranslationSubstring(dictionary.Index, query);

            return byText.Concat(byTranslation);
        }

        public (IEnumerable<FreeExpression> unidirectional, IEnumerable<FreeExpression> opposite) GetMatchingExpressionsBidir(String languageIn, String languageOut, String query)
        {
            var unidirectional = GetMatchingExpressions(languageIn, languageOut, query);
            var opposite = GetMatchingExpressions(languageOut, languageIn, query);

            return (unidirectional, opposite);
        }
    }
}

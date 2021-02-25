using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using Service.Mapper;
using Service.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class TranslationService : ServiceBase, ITranslationService
    {
        private readonly IEntryRepository entries;
        private readonly IFreeExpressionRepository expressions;
        private readonly IDictionaryRepository dictionaries;
        private readonly IMeaningRepository meanings;

        public TranslationService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
            this.entries = uow.Entries;
            this.expressions = uow.FreeExpressions;
            this.dictionaries = uow.Dictionaries;
            this.meanings = uow.Meanings;
        }

        //unidirectional
        public TranslationResponse Translate(String dictionary, String query)
        {
            var (langIn, langOut) = GetLanguages(dictionary);
            if (EnsureDictionaryExists(langIn, langOut))
                throw new KeyNotFoundException("Dictionary does not exist");

            return new()
            {
                ResultEntries = GetMatchingEntries(langIn, langOut, query).Select(Map),
                ResultExpressions = GetMatchingExpressions(langIn, langOut, query).Select(Map),
            };
        }

        public BidirectionalTranslationResponse TranslateBidir(String dictionaries, String query)
        {
            var (langIn, langOut) = GetLanguages(dictionaries);
            if (EnsureDictionaryExists(langIn, langOut, true))
                throw new KeyNotFoundException("Dictionary does not exist");

            return new()
            {
                BaseResultEntries = GetMatchingEntries(langIn, langOut, query).Select(Map),
                OppositeResultEntries = GetMatchingEntries(langOut, langIn, query).Select(Map),
                BaseResultExpressions = GetMatchingExpressions(langIn, langOut, query).Select(Map),
                OppositeResultExpressions = GetMatchingExpressions(langOut, langIn, query).Select(Map)
            };
        }

        private (String langIn, String langOut) GetLanguages(String dictionaries)
        {
            var arr = dictionaries.Split('-');
            return (arr[0], arr[1]);
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

        //then query free expressions
        public IEnumerable<FreeExpression> GetMatchingExpressions(String languageIn, String languageOut, String query)
        {
            var dictionary = dictionaries.GetByLanguageInAndOut(languageIn, languageOut);
            //we should return all FreeExpressions that contain the query both in Text and in Translation
            var byText = expressions.GetByDictionaryAndTextSubstring(dictionary.Index, query);
            var byTranslation = expressions.GetByDictionaryAndTranslationSubstring(dictionary.Index, query);

            return byText.Concat(byTranslation);
        }

        private GetEntry Map(Entry e) => mapper.Map<Entry, GetEntry>(e);
        private GetFreeExpression Map(FreeExpression f) => mapper.Map<FreeExpression, GetFreeExpression>(f);
    }
}

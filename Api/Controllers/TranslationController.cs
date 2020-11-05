using Data.Dto;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Repository;
using System;
using System.Collections.Generic;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/translate")]
    public class TranslationController : Controller
    {
        private readonly UnitOfWork uow;

        public TranslationController(UnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        [Route("api/translate/{dictionary}/{query}")]
        public TranslationResponse Get(String dictionaryName, String query, bool bidir = false)
        {
            //so the dictionary string is in format <language in name>-<language out name>
            var arr = query.Split('-');
            var (langIn, langOut) = (arr[0], arr[1]);

            var dictionary = uow.Dictionaries.GetByLanguageInAndOut(langIn, langOut);
            var oppositeDictionary = uow.Dictionaries.GetByLanguageInAndOut(langOut, langIn);

            var dicResult = GetForDictionary(dictionary, query);
            var oppositeResult = GetForDictionary(oppositeDictionary, query);

            //combine these results if is bidirectional
            // 1. Two separate lists for Entries (for this and that dictionary)
            // 2. If meanings are found, get their Entries and add to these lists
            // 3. FreeExpressions stay free (two separate lists for them)

            return Combine(dicResult, oppositeResult);
        }

        private TranslationResponse GetForDictionary(Dictionary dictionary, String query)
        {
            //query entries in search of word
            var similarEntries = uow.Entries.GetByDictionaryAndWord(dictionary.Index, query);

            //then query meanings in search of meaning //or should check the opposite side of the dictionary
            var similarMeanings = uow.Meanings.GetByDictionaryAndValueSubstring(dictionary.Index, query);

            //then query for examples in meanings
            //then query free expressions
            var similarExpressions = uow.Expressions.GetByDictionaryTextSubstring(dictionary.Index, query);

            return null;
        }

        private TranslationResponse Combine(TranslationResponse one, TranslationResponse opposite)
        {
            //combine them according to the rules up
            return null;
        }
    }
}

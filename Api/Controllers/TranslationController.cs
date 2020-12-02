using AutoMapper;
using Data.Dto;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/translate")]
    public class TranslationController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public TranslationController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("{dictionaryName}/{query}")]
        public ActionResult<object> Get(String dictionaryName, String query, bool bidir = false)
        {
            //so the dictionary string is in format <language in name>-<language out name>
            var arr = query.Split('-');
            var (langIn, langOut) = (arr[0], arr[1]);

            var dictionary = uow.Dictionaries.GetByLanguageInAndOut(langIn, langOut);
            var dicResult = GetForDictionary(dictionary, query);


            if (bidir)
            {
                var oppositeDictionary = uow.Dictionaries.GetByLanguageInAndOut(langOut, langIn);
                var oppositeResult = GetForDictionary(oppositeDictionary, query);
                //dicResult = Combine(dicResult, oppositeResult);
            }

            //combine these results if is bidirectional
            // 1. Two separate lists for Entries (for this and that dictionary)
            // 2. If meanings are found, get their Entries and add to these lists
            // 3. FreeExpressions stay free (two separate lists for them)

            //return Combine(dicResult, oppositeResult);

            return new object[]{dictionaryName, query, bidir.ToString()};
        }

        private TranslationResult GetForDictionary(Dictionary dictionary, String query)
        {
            //query entries in search of word
            var similarEntries = uow.Entries.GetByDictionaryAndWord(dictionary.Index, query);

            //then query for examples in meanings
            //TODO i think that method should be moved to EntryRepository because we want to get entries.
            //When getting these examples we would have to get their meanings and the meanings' entries.
            //So now, let's assume we ommit that and that these are gonna be included with similarEntries
            //var similarExamples = uow.Examples.GetByDictionaryAndTextSubstring(dictionary.Index, query);
            //TODO another thing - do we want to query by meanings' values in the opposite dictionary?
            //I don't think that's necessary but I'll play with the API and decide then.
            //TODO also, export it to some service class

            //then query free expressions
            var similarExpressions = uow.FreeExpressions.GetByDictionaryAndTextSubstring(dictionary.Index, query);

            //TODO do that mapping in some other place
            return new TranslationResult
            {
                ResultEntries = similarEntries.Select(e => mapper.Map<Entry, GetEntry>(e)),
                ResultExpressions = similarExpressions.Select(f => mapper.Map<FreeExpression, GetFreeExpression>(f))
            };
            
        }

        private BidirectionalTranslationResponse Combine(TranslationResult one, TranslationResult opposite)
        {
            var result = new BidirectionalTranslationResponse
            {
                ResultEntries = one.ResultEntries,
                ResultExpressions = one.ResultExpressions,
                OppositeResultEntries = opposite.ResultEntries,
                OppositeResultExpressions = opposite.ResultExpressions,
            };

            return result;
        }
    }
}

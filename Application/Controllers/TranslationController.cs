using Data.Dto;
using Data.Mapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Repository;
using System;
using System.Linq;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/translate")]
    public class TranslationController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITranslationService service;

        public TranslationController(IMapper mapper, ITranslationService service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        [HttpGet]
        [Route("{dictionaryName}/{query}")]
        public IActionResult Get(String dictionaryName, String query, bool bidir = false)
        {
            //so the dictionary string is in format <language in name>-<language out name>
            var arr = dictionaryName.Split('-');
            var (langIn, langOut) = (arr[0], arr[1]);

            var exists = service.EnsureDictionaryExists(langIn, langOut, bidir);
            if (!exists)
                return NotFound();

            if (bidir)
            {
                var (baseEntries, oppositeEntries) = service.GetMatchingEntriesBidir(langIn, langOut, query);
                var (baseExpressions, oppositeExpressions) = service.GetMatchingExpressionsBidir(langIn, langOut, query);

                var bidirResponse = new BidirectionalTranslationResponse
                {
                    BaseResultEntries = baseEntries.Select(EntryToDto),
                    BaseResultExpressions = baseExpressions.Select(ExpressionToDto),
                    OppositeResultEntries = oppositeEntries.Select(EntryToDto),
                    OppositeResultExpressions = oppositeExpressions.Select(ExpressionToDto),
                };

                return Ok(bidirResponse);
            }

            var entriesFound = service.GetMatchingEntries(langIn, langOut, query);
            var expressionsFound = service.GetMatchingExpressions(langIn, langOut, query);

            var response = new TranslationResponse
            {
                ResultEntries = entriesFound.Select(EntryToDto),
                ResultExpressions = expressionsFound.Select(ExpressionToDto)
            };

            return Ok(response);
        }

        private GetEntry EntryToDto(Entry e) => mapper.Map<Entry, GetEntry>(e);
        private GetFreeExpression ExpressionToDto(FreeExpression f) => mapper.Map<FreeExpression, GetFreeExpression>(f);
    }
}

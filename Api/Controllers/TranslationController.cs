using Data.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Text;

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
        public IEnumerable<SomeTranslateResponseModel> Get(String dictionary, String query, bool bidir = false)
        {
            //but even earlier, decode dictionary string into language names

            //first off, check if is bidir
            //then query entries in search of word
            //then query meanings in search of meaning //should be bidir actually //or should check the opposite side of the dictionary
            //then query for examples in meanings
            //then query free expressions

            return null;
        }
    }
}

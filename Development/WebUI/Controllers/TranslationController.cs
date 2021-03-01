using Microsoft.AspNetCore.Mvc;
using Service;
using System;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/translate")]
    public class TranslationController : Controller
    {
        private readonly ITranslationService service;

        public TranslationController(ITranslationService service)
        {
            this.service = service;
        }
        //TODO throw these parameters into body
        /// <summary>
        /// Retrieves all entries and free expression that contain given word.
        /// </summary>
        /// <remarks>
        /// <paramref name="dictionaryName"/> Is a string containing dictionary language in and out (in format "<langIn>-<langOut>") <br/>
        /// <paramref name="query"/> The word we're looking for. <br/>
        /// <paramref name="bidir"/> If set to true, search is performed also in the opposite dictionary. <br/>
        /// </remarks>
        [HttpGet]
        [Route("{dictionaryName}/{query}")]
        public IActionResult Get(String dictionaryName, String query, bool bidir = false)
        {
            //so the dictionary string is in format <language in name>-<language out name>
            if (bidir)
                return Ok(service.TranslateBidir(dictionaryName, query));
            return Ok(service.Translate(dictionaryName, query));
        }
    }
}

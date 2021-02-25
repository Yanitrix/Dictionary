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

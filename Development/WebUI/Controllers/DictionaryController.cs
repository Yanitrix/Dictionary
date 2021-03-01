using Domain.Dto;
using Service.Mapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/dictionary")]
    public class DictionaryController : Controller
    {
        private readonly IDictionaryService service;
        private readonly IMapper mapper;

        public DictionaryController(IDictionaryService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        //TODO maybe return all with counted entries and expressions?
        /// <summary>
        /// Retrieves dictionary list that contain specified languages.
        /// </summary>
        /// <remarks>
        /// If <paramref name="langIn"/> and <paramref name="langOut"/> are present, returns a dictionary that includes those languages. <br />
        /// If either <paramref name="langIn"/> or <paramref name="langOut"/> is present, returns a list of dictionaries with specified language. <br />
        /// if <paramref name="lang"/> is present, returns a list of dictionaries that contain this language, whether it's language in or language out. <br />
        /// </remarks>
        /// <param name="langIn"></param>
        /// <param name="langOut"></param>
        /// <param name="lang"></param>
        [HttpGet]
        public IEnumerable<GetDictionary> Index(String langIn = null, String langOut = null, String lang = null)
        {
            return service.GetContainingLanguage(langIn, langOut, lang);
        }

        /// <summary>
        /// Retrieves a dictionary with specified index.
        /// </summary>
        [HttpGet("{index}")]
        public ActionResult<GetDictionary> Get(int index)
        {
            var found = service.Get(index);
            if (found == null)
                return NotFound();
            return found;
        }

        /// <summary>
        /// Created a new dictionary.
        /// </summary>
        /// <response code="201">Dictionary created succesfully</response>
        /// <response code="400">Languages are not found / dictionary already exists</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateDictionary dto)
        {
            var result = service.Add(dto);
            if (!result.IsValid)
                return BadRequest(result);
            var response = ToDto(result.Entity as Dictionary);
            return Created("api/dictionary/" + response.Index, response);
        }

        /// <summary>
        /// Deletes an existing dictionary.
        /// </summary>
        /// <response code="204">Dictionary deleted succesfully</response>
        /// <response code="404">Dictionary with given index is not found</response>
        [HttpDelete("{index}")]
        public IActionResult Delete(int index)
        {
            var result = service.Delete(index);
            if (!result.IsValid)
                return NotFound(result);
            return NoContent();
        }

        private GetDictionary ToDto(Dictionary d) => mapper.Map<Dictionary, GetDictionary>(d);
    }
}

using Domain.Dto;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Domain.Commands;
using Domain.Queries;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/dictionary")]
    public class DictionaryController : Controller
    {
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
        public IEnumerable<GetDictionary> Index(
            [FromServices] IQueryHandler<DictionaryContainingLanguagesQuery, IEnumerable<GetDictionary>> handler,
            String langIn = null, String langOut = null, String lang = null)
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = lang,
                LanguageIn = langIn,
                LanguageOut = langOut
            };

            return handler.Handle(query);
        }

        /// <summary>
        /// Retrieves a dictionary with specified index.
        /// </summary>
        [HttpGet("{index}")]
        public ActionResult<GetDictionary> Get(int index, [FromServices] IQueryHandler<DictionaryByIndexQuery, GetDictionary> handler)
        {
            var query = new DictionaryByIndexQuery(index);
            return handler.Handle(query) ?? (ActionResult<GetDictionary>) NotFound();
        }

        /// <summary>
        /// Created a new dictionary.
        /// </summary>
        /// <response code="201">Dictionary created successfully</response>
        /// <response code="400">Languages are not found / dictionary already exists</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateDictionaryCommand dto, [FromServices] ICommandHandler<CreateDictionaryCommand, GetDictionary> handler)
        {
            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Created("api/dictionary/" + response.Entity.Index, response.Entity);
            return BadRequest(response.Errors);
        }

        /// <summary>
        /// Deletes an existing dictionary.
        /// </summary>
        /// <response code="204">Dictionary deleted successfully</response>
        /// <response code="404">Dictionary with given index is not found</response>
        [HttpDelete("{index}")]
        public IActionResult Delete(int index, [FromServices] ICommandHandler<DeleteDictionaryCommand, Dictionary> handler)
        {
            var command = new DeleteDictionaryCommand
            {
                PrimaryKey = index
            };

            var result = handler.Handle(command);
            if (result.IsSuccessful)
                return NoContent();
            return NotFound(result.Errors);
        }
    }
}

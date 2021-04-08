using Domain.Dto;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Commands;
using Domain.Queries;
using Domain.Repository;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/language")]
    public class LanguageController : Controller
    {

        //TODO should test it with bulk data
        /// <summary>
        /// Retrieves all languages with their word count
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<LanguageWordCount>> Index([FromServices] ILanguageRepository repo)
        {
            return repo.AllWithWordCount().ToList();
        }

        [HttpGet("{name}")]
        public ActionResult<GetLanguage> Get(String name, [FromServices] IQueryHandler<LanguageByNameQuery, GetLanguage> handler)
        {
            var query = new LanguageByNameQuery(name);
            return handler.Handle(query) ?? (ActionResult<GetLanguage>) NotFound();
        }

        /// <summary>
        /// Creates a language
        /// </summary>
        /// <response code="201">Language created successfully</response>
        /// <response code="400">Model invalid or related entities not found</response>
        [HttpPost]
        public IActionResult Create([FromBody] CreateLanguageCommand dto, [FromServices] ICommandHandler<CreateLanguageCommand, GetLanguage> handler)
        {
            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Created("api/language/" + response.Entity.Name, response.Entity);
            return BadRequest(response.Errors);
        }

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{name}")]
        public IActionResult Delete(String name, [FromServices] ICommandHandler<DeleteLanguageCommand, Language> handler)
        {
            var command = new DeleteLanguageCommand
            {
                PrimaryKey = name
            };

            var response = handler.Handle(command);
            if (response.IsSuccessful)
                return NoContent();
            return NotFound(response.Errors);
        }
    }
}

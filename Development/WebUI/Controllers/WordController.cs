using Domain.Dto;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Commands;
using Domain.Queries;
using static WebUI.Utils.ErrorMessages;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/word")]
    public class WordController : Controller
    {
        //some algorithm to find similar words when non exact are found?
        /// <summary>
        /// Retrieves all words with given value. Case insensitive 
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<GetWord>> Get(String value, [FromServices] IQueryHandler<WordByValueQuery, IEnumerable<GetWord>> handler)
        {
            var query = new WordByValueQuery(value);
            //It queries by the exact value, not substring
            return handler.Handle(query).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<GetWord> Get(int id, [FromServices] IQueryHandler<WordByIdQuery, GetWord> handler)
        {
            var query = new WordByIdQuery(id);
            return handler.Handle(query) ?? (ActionResult<GetWord>) NotFound();
        }

        /// <summary>
        /// Creates a new word.
        /// </summary>
        /// <response code="201">Word created successfully</response>
        /// <response code="400">Model invalid/related entities not found</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateWordCommand dto, [FromServices] ICommandHandler<CreateWordCommand, GetWord> handler)
        {
            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Created("api/word/" + response.Entity.ID, response.Entity);
            return BadRequest(dto);
        }

        /// <summary>
        /// Updates a word. Not all values can be updated.
        /// </summary>
        /// <response code="200">Update successful</response>
        /// <response code="400">Model invalid/related entities not found</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateWordCommand dto, [FromServices] ICommandHandler<UpdateWordCommand, GetWord> handler)
        {
            if (id != dto.ID)
                return BadRequest(ROUTE_PARAMETER_NOT_MATCH);

            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Ok();
            return BadRequest(response.Errors);
        }
        /// <summary>
        /// Deletes a word
        /// </summary>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] ICommandHandler<DeleteWordCommand, Word> handler)
        {
            var command = new DeleteWordCommand
            {
                PrimaryKey = id
            };

            var response = handler.Handle(command);
            if (response.IsSuccessful)
                return NoContent();
            return NotFound(response.Errors);
        }
    }
}

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
    [Route("api/entry")]
    public class EntryController : Controller
    {
        //TODO some pagination maybe?
        /// <summary>
        /// Retrieves all entries containing <paramref name="word"/> or belonging to a dictionary with given <paramref name="dictionaryIndex"/> 
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<GetEntry>> Get(String word, int? dictionaryIndex, [FromServices] IQueryHandler<EntryByWordAndDictionaryQuery, IEnumerable<GetEntry>> handler)
        {
            var query = new EntryByWordAndDictionaryQuery
            {
                WordValue = word,
                DictionaryIndex = dictionaryIndex
            };

            return handler.Handle(query).ToList();
        }

        /// <summary>
        /// Retrieves an entry with given id
        /// </summary>
        /// <response code="404">If entry with given id does not exist</response>
        [HttpGet("{id}")]
        public ActionResult<GetEntry> Get(int id, [FromServices] IQueryHandler<EntryByIdQuery, GetEntry> handler)
        {
            var query = new EntryByIdQuery(id);
            return handler.Handle(query) ?? (ActionResult<GetEntry>) NotFound();
        }

        /// <summary>
        /// Creates an entry with given values. After creation of the entity, not all values can be updated.
        /// </summary>
        /// <response code="201">Entry created successfully</response>
        /// <response code="404">Model is invalid or related entities not found</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateEntryCommand dto, [FromServices] ICommandHandler<CreateEntryCommand, GetEntry> handler)
        {
            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Created("api/entry/" + response.Entity.ID, response.Entity);
            return BadRequest(response);
        }

        /// <summary>
        /// Updates entry with new values. Not all values can be updated.
        /// </summary>
        /// <response code="200">Update successful</response>
        /// <response code="400">Model is invalid or related entities not found</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateEntryCommand dto, [FromServices] ICommandHandler<UpdateEntryCommand, GetEntry> handler)
        {
            if (id != dto.ID)
                return BadRequest(ROUTE_PARAMETER_NOT_MATCH);
            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Ok();
            return BadRequest(dto);
        }

        /// <summary>
        /// Deletes entry with given id
        /// </summary>
        /// <response code="204">Deletion succesful</response>
        /// <response code="404">Entry with given id not found</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] ICommandHandler<DeleteEntryCommand, Entry> handler)
        {
            var command = new DeleteEntryCommand
            {
                PrimaryKey = id
            };

            var response = handler.Handle(command);
            if (response.IsSuccessful)
                return NoContent();
            return NotFound();
        }
    }
}

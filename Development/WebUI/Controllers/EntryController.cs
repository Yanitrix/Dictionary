using Domain.Dto;
using Service.Mapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static WebUI.Utils.ErrorMessages;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/entry")]
    public class EntryController : Controller
    {
        private readonly IEntryService service;
        private readonly IMapper mapper;

        public EntryController(IEntryService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        //TODO some pagination maybe?
        /// <summary>
        /// Retrieves all entriex containg <paramref name="word"/> or belonging to a dictionary with given <paramref name="index"/> 
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<GetEntry>> Get(String word, int? dictionaryIndex)
        {
            return service.GetByDictionaryAndWord(word, dictionaryIndex).ToArray();
        }

        /// <summary>
        /// Retrieves an entry with given id
        /// </summary>
        /// <response code="404">If entry with given id does not exist</response>
        [HttpGet("{id}")]
        public ActionResult<GetEntry> Get(int id)
        {
            var found = service.Get(id);
            if (found == null)
                return NotFound();
            return found;
        }

        /// <summary>
        /// Creates an entry with given values. After creation of the entity, not all values can be updated.
        /// </summary>
        /// <response code="201">Entry created successfully</response>
        /// <response code="404">Model is invalid or related entities not found</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateEntry dto)
        {
            var result = service.Add(dto);
            if (!result.IsValid)
                return BadRequest(result);
            var response = ToDto(result.Entity as Entry);
            return Created("api/entry/" + response.ID, response);
        }

        /// <summary>
        /// Updates entry with new values. Not all values can be updated.
        /// </summary>
        /// <response code="200">Update sussesful</response>
        /// <response code="400">Model is invalid or related entities not found</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateEntry dto)
        {
            if (id != dto.ID)
                return BadRequest(ROUTE_PARAMETER_NOT_MATCH);
            var result = service.Update(dto);
            if (!result.IsValid)
                return BadRequest(result);
            return Ok();
        }

        /// <summary>
        /// Deletes entry with given id
        /// </summary>
        /// <response code="204">Deletion succesful</response>
        /// <response code="404">Entry with given id not found</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = service.Delete(id);
            if (!result.IsValid)
                return NotFound(result);
            return NoContent();
        }

        private GetEntry ToDto(Entry e) => mapper.Map<Entry, GetEntry>(e);
    }
}

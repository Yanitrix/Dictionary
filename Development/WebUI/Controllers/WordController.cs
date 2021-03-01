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
    [Route("api/word")]
    public class WordController : Controller
    {
        private readonly IWordService service;
        private readonly IMapper mapper;

        public WordController(IWordService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        //some algorithm to find similar words when non exact are found?
        /// <summary>
        /// Retrieves all words with given value. Case insenstitive 
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<GetWord>> Get(String value)
        {
            if (value == null)
                return Array.Empty<GetWord>();
            //It queries by the exact value, not substring
            var words = service.Get(value);
            return words.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<GetWord> Get(int id)
        {
            var found = service.Get(id);
            if (found == null)
                return NotFound();
            return found;
        }

        /// <summary>
        /// Creates a new word.
        /// </summary>
        /// <response code="201">Word created successfully</response>
        /// <response code="400">Model invalid/related entities not found</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateWord dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Utils.RemoveRedundantWhitespaces(entity.Properties);

            var result = service.Add(dto);
            if (!result.IsValid)
                return BadRequest(result);
            var response = ToDto(result.Entity as Word);
            return Created("api/word/" + response.ID, response);
        }

        /// <summary>
        /// Updates a word. Not all values can be updated.
        /// </summary>
        /// <response code="200">Update successful</response>
        /// <response code="400">Model invalid/related entities not found</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateWord dto)
        {
            if (id != dto.ID)
                return BadRequest(ROUTE_PARAMETER_NOT_MATCH);
            //TODO move that into service
            //Utils.RemoveRedundantWhitespaces(entity.Properties);

            var result = service.Update(dto);
            if (!result.IsValid)
                return BadRequest(result);
            return Ok();
        }
        /// <summary>
        /// Deletes a word
        /// </summary>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = service.Delete(id);
            if (!result.IsValid)
                return NotFound(result);
            return NoContent();
        }

        private GetWord ToDto(Word word) => mapper.Map<Word, GetWord>(word);
    }
}

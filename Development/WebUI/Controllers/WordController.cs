using Domain.Dto;
using Service.Mapper;
using Domain.Models;
using Domain.Util;
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateWord dto)
        {
            if (id != dto.ID)
                return BadRequest(ROUTE_PARAMETER_NOT_MATCH);

            //Utils.RemoveRedundantWhitespaces(entity.Properties);

            var result = service.Update(dto);
            if (!result.IsValid)
                return BadRequest(result);
            return Ok();
        }

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

using Data.Dto;
using Data.Mapper;
using Data.Models;
using Data.Util;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Controllers
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
            return words.Select(ToDto).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<GetWord> Get(int id)
        {
            var word = service.Get(id);
            if (word == null) return NotFound();

            return ToDto(word);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateWord dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = ToWord(dto);
            Utils.RemoveRedundantWhitespaces(entity.Properties);

            var result = service.Add(entity);
            if (!result.IsValid)
                return BadRequest(result);

            return Created("api/word/" + entity.ID, ToDto(entity));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateWord dto)
        {
            var entity = ToWord(dto);
            entity.ID = id;
            Utils.RemoveRedundantWhitespaces(entity.Properties);

            var result = service.Update(entity);
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

        private Word ToWord(CreateWord dto) => mapper.Map<CreateWord, Word>(dto);
        private Word ToWord(UpdateWord dto) => mapper.Map<UpdateWord, Word>(dto);

        private GetWord ToDto(Word word) => mapper.Map<Word, GetWord>(word);

    }
}

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

namespace Api.Controllers
{
    [ApiController]
    [Route("api/word")]
    public class WordController : Controller
    {

        private readonly IService<Word> service;
        private readonly IWordRepository repo;
        private readonly IMapper mapper;

        public WordController(IService<Word> service, IWordRepository repo, IMapper mapper)
        {
            this.service = service;
            this.repo = repo;
            this.mapper = mapper;
        }

        //some algorithm to find similar words when non exact are found?
        [HttpGet]
        public ActionResult<IEnumerable<GetWord>> Get(String value)
        {
            if (value == null)
                return new List<GetWord>();
            //It queries by the exact value, not substring
            var words = repo.GetByValue(value, true);
            return words.Select(ToDto).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<GetWord> Get(int id)
        {
            var word = repo.GetByID(id);
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

            var result = service.TryAdd(entity);
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

            var result = service.TryUpdate(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var word = repo.GetByID(id);
            if (word == null)
                return NotFound();
            repo.Delete(word);
            return NoContent();
        }

        private Word ToWord(CreateWord dto) => mapper.Map<CreateWord, Word>(dto);
        private Word ToWord(UpdateWord dto) => mapper.Map<UpdateWord, Word>(dto);

        private GetWord ToDto(Word word) => mapper.Map<Word, GetWord>(word);

    }
}

using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Util;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Text;

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

        //[HttpGet]
        //All() //won't do that because it would cause a heavy request for all words

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var word = repo.GetByID(id);
            if (word == null) return NotFound();

            return Json(ToDto(word));
        }

        [HttpPost]
        public IActionResult Post([FromBody] WordDto dto)
        {
            var entity = ToWord(dto);
            Utils.RemoveRedundantWhitespaces(entity.Properties);

            var result = service.TryAdd(entity);
            if (!result.IsValid)
                return BadRequest(result);

            return Created("api/word/" + entity.ID, entity);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] WordDto dto)
        {
            var entity = ToWord(dto);
            entity.ID = id;
            Utils.RemoveRedundantWhitespaces(entity.Properties);

            var result = service.TryUpdate(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Ok("Resource updated succesfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var word = repo.GetByID(id);
            if (word == null) return NotFound();
            repo.Delete(word);
            return NoContent();
        }

        private Word ToWord(WordDto dto) => mapper.Map<WordDto, Word>(dto);

        private WordDto ToDto(Word word) => mapper.Map<Word, WordDto>(word);

    }
}

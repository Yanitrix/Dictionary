using Data.Dto;
using Data.Mapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/entry")]
    public class EntryController : Controller
    {
        private readonly IService<Entry> service;
        private readonly IEntryRepository repo;
        private readonly IMapper mapper;

        public EntryController(IService<Entry> service, IEntryRepository repo, IMapper mapper)
        {
            this.service = service;
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GetEntry>> Get(String word, int? dictionaryIndex)
        {
            if (word == null && dictionaryIndex == null)
                return new List<GetEntry>();
            if (word != null && dictionaryIndex != null)
                return repo.GetByDictionaryAndWord(dictionaryIndex.Value, word).Select(ToDto).ToList();
            if (word != null)
                return repo.GetByWord(word).Select(ToDto).ToList();
            return repo.GetByDictionary(dictionaryIndex.Value).Select(ToDto).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<GetEntry> Get(int id)
        {
            var entry = repo.GetByID(id);
            if (entry == null)
                return NotFound();
            return ToDto(entry);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateOrUpdateEntry dto)
        {
            var entry = ToEntity(dto);
            var result = service.TryAdd(entry);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/entry/" + entry.ID, ToDto(entry));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateOrUpdateEntry dto)
        {
            var entry = ToEntity(dto);
            entry.ID = id;
            var result = service.TryUpdate(entry);
            if (!result.IsValid)
                return BadRequest(result);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entry = repo.GetByID(id);
            if (entry == null)
                return NotFound();
            repo.Delete(entry);
            return NoContent();
        }

        private Entry ToEntity(CreateOrUpdateEntry dto) => mapper.Map<CreateOrUpdateEntry, Entry>(dto);
        private GetEntry ToDto(Entry e) => mapper.Map<Entry, GetEntry>(e);
    }
}

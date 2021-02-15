using Domain.Dto;
using Service.Mapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;

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
        [HttpGet]
        public ActionResult<IEnumerable<GetEntry>> Get(String word, int? dictionaryIndex)
        {
            return service.GetByDictionaryAndWord(word, dictionaryIndex).Select(ToDto).ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<GetEntry> Get(int id)
        {
            var entry = service.Get(id);
            if (entry == null)
                return NotFound();
            return ToDto(entry);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateOrUpdateEntry dto)
        {
            var entry = ToEntity(dto);
            var result = service.Add(entry);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/entry/" + entry.ID, ToDto(entry));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateOrUpdateEntry dto)
        {
            var entry = ToEntity(dto);
            entry.ID = id;
            var result = service.Update(entry);
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

        private Entry ToEntity(CreateOrUpdateEntry dto) => mapper.Map<CreateOrUpdateEntry, Entry>(dto);
        private GetEntry ToDto(Entry e) => mapper.Map<Entry, GetEntry>(e);
    }
}

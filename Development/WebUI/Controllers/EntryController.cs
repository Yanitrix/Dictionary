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
        public IActionResult Post([FromBody] CreateEntry dto)
        {
            var result = service.Add(dto);
            if (!result.IsValid)
                return BadRequest(result);
            var response = ToDto(result.Entity as Entry);
            return Created("api/entry/" + response.ID, response);
        }

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

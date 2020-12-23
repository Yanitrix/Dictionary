using Data.Dto;
using Data.Mapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Repository;

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

        //[HttpGet]
        //All()? i dont think so

        //TODO querying entry by word value
        //TODO wordproperties are not included with word
        [HttpGet("{id}")]
        public ActionResult<GetEntry> Get(int id)
        {
            //TODO include meanings and examples
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

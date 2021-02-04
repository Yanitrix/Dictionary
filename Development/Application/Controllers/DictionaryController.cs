using Data.Dto;
using Data.Mapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/dictionary")]
    public class DictionaryController : Controller
    {

        private readonly IDictionaryService service;
        private readonly IMapper mapper;

        public DictionaryController(IDictionaryService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        //TODO maybe return all with counted entries and expressions?
        [HttpGet]
        public IEnumerable<GetDictionary> Index(String langIn = null, String langOut = null, String lang = null)
        {
            return service.GetContainingLanguage(langIn, langOut, lang).Select(ToDto);
        }

        [HttpGet("{index}")]
        public ActionResult<GetDictionary> Get(int index)
        {
            var entity = service.Get(index);
            if (entity == null)
                return NotFound();
            return ToDto(entity);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateDictionary dto)
        {
            var entity = ToEntity(dto);
            var result = service.Add(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/dictionary" + entity.Index, ToDto(entity));
        }

        [HttpDelete("{index}")]
        public IActionResult Delete(int index)
        {
            var result = service.Delete(index);
            if (!result.IsValid)
                return NotFound(result);
            return NoContent();
        }

        private GetDictionary ToDto(Dictionary d) => mapper.Map<Dictionary, GetDictionary>(d);
        private Dictionary ToEntity(CreateDictionary dto) => mapper.Map<CreateDictionary, Dictionary>(dto);
    }
}

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
            return service.GetContainingLanguage(langIn, langOut, lang);
        }

        [HttpGet("{index}")]
        public ActionResult<GetDictionary> Get(int index)
        {
            var found = service.Get(index);
            if (found == null)
                return NotFound();
            return found;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateDictionary dto)
        {
            var result = service.Add(dto);
            if (!result.IsValid)
                return BadRequest(result);
            var response = ToDto(result.Entity as Dictionary);
            return Created("api/dictionary/" + response.Index, response);
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
    }
}

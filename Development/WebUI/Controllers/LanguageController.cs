using Domain.Dto;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Service;
using System.Linq;
using Service.Mapper;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/language")]
    public class LanguageController : Controller
    {
        private readonly ILanguageService service;
        private readonly IMapper mapper;

        public LanguageController(ILanguageService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        //TODO should test it with bulk data
        [HttpGet]
        public ActionResult<IEnumerable<LanguageWordCount>> Index()
        {
            return service.AllWithWordCount().ToList();
        }

        [HttpGet("{name}")]
        public ActionResult<GetLanguage> Get(String name)
        {
            Language entity = service.Get(name);

            if (entity == null)
                return NotFound($"Language with Name: \"{name}\" doesn't exist");

            return ToDto(entity);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateLanguage dto)
        {
            var result = service.Add(dto);
            if (!result.IsValid)
                return BadRequest(result);
            var response = ToDto(result.Entity as Language);
            return Created("api/language/" + response.Name, response);
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(String name)
        {
            var result = service.Delete(name);
            if (!result.IsValid)
                return NotFound(result);
            return NoContent();
        }

        private GetLanguage ToDto(Language entity) => mapper.Map<Language, GetLanguage>(entity);
    }
}

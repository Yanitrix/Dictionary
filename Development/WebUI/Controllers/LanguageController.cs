using Domain.Dto;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Service;
using System.Linq;
using Service.Mapper;

namespace Application.Controllers
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
            var entity = ToEntity(dto);

            var result = service.Add(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/language/" + entity.Name, ToDto(entity));
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(String name)
        {
            var result = service.Delete(name);
            if (!result.IsValid)
                return NotFound(result);
            return NoContent();
        }

        private Language ToEntity(CreateLanguage dto) => mapper.Map<CreateLanguage, Language>(dto);
        private GetLanguage ToDto(Language entity) => mapper.Map<Language, GetLanguage>(entity);
    }
}

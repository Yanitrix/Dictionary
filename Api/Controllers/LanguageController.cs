using Data.Dto;
using AutoMapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Service.Repository;
using Service;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/language")]
    public class LanguageController : Controller
    {
        private readonly IService<Language> service;
        private readonly ILanguageRepository repo;
        private readonly IMapper mapper;

        public LanguageController(ILanguageRepository repo, IService<Language> service, IMapper mapper)
        {
            this.service = service;
            this.repo = repo;
            this.mapper = mapper;
        }

        //TODO should test it with bulk data
        [HttpGet]
        public ActionResult<IEnumerable<LanguageWordCount>> Index()
        {
            return repo.AllWithWordCount().ToList();
        }

        //todo write response dto that includes words
        [HttpGet("{name}")]
        public ActionResult<GetLanguage> Get(String name, bool withWords = false)
        {
            Language entity = withWords ? repo.GetByNameWithWords(name) : repo.GetByName(name);

            if (entity == null)
                return NotFound($"Language with Name: \"{name}\" doesn't exist");

            return ToDto(entity);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateLanguage dto)
        {
            var entity = ToEntity(dto);

            var result = service.TryAdd(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/language/" + entity.Name, ToDto(entity));
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(String name)
        {
            var entity = repo.GetByName(name);
            if (entity == null) 
                return NotFound($"Language with Name: \"{name}\" doesn't exist");

            repo.Delete(entity);
            return NoContent();
        }

        private Language ToEntity(CreateLanguage dto) => mapper.Map<CreateLanguage, Language>(dto);
        private GetLanguage ToDto(Language entity) => mapper.Map<Language, GetLanguage>(entity);
    }
}

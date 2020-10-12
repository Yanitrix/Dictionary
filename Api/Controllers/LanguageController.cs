using Data.Dto;
using AutoMapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Service.Repository;
using Service;

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

        public IEnumerable<LanguageWordCount> Index()
        {
            return repo.AllWithWordCount();
        }

        [HttpGet("{name}")]
        public IActionResult Get(String name, bool withWords)
        {
            Language entity = (withWords) ? repo.GetByNameWithWords(name) : repo.GetByName(name);

            if (entity == null) return NotFound($"Language with Name: \"{name}\" doesn't exist");

            return Json(entity);
        }

        [HttpPost]
        public IActionResult Create([FromBody] String name)
        {
            var entity = new Language
            {
                Name = name
            };

            var result = service.TryAdd(entity);
            if (result.IsValid) return Created("api/language/" + entity.Name, entity);
            else return BadRequest(result);
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(String name)
        {
            var entity = repo.GetByName(name);
            if (entity == null) return NotFound($"Language with Name: \"{name}\" doesn't exist");

            repo.Delete(entity);

            return NoContent();
        }
    }
}

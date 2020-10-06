using Data.Dto;
using AutoMapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Service.Repository;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/language")]
    public class LanguageController : Controller

    {
        private readonly ILanguageRepository service;
        private readonly IMapper mapper;

        public LanguageController(ILanguageRepository service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public IEnumerable<LanguageDto> Index()
        {
            return service.All().Select(mapper.Map<Language, LanguageDto>).ToList();
        }

        [HttpPost]
        public IActionResult Create([FromBody] String name)
        {
            return null;
        }

        [HttpGet("{name}")]
        public IActionResult Get(String name)
        {
            var entity = service.GetByName(name);
            if (entity == null) return NotFound($"Language with name {name} doesn't exist");

            return Json(entity);
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(String name)
        {
            var entity = service.GetByName(name);
            if (entity == null) return NotFound($"Language with name {name} doesn't exist");

            service.Delete(entity);

            return NoContent();
        }
    }
}

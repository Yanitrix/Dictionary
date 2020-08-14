using Api.Dto;
using Api.Service;
using Api.Validation;
using AutoMapper;
using Dictionary_MVC.Data;
using Dictionary_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Controllers
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

        public IEnumerable<LanguageDto> Index()
        {
            return service.All().Select(mapper.Map<Language, LanguageDto>).ToList();
        }

        [HttpPost]
        public IActionResult Create([FromBody] String name)
        {
            service.ValidationDictionary = new ValidationDictionary(ModelState);

            Language entity = new Language
            {
                Name = name,
            };

            if (!service.IsReadyToAdd(entity)) return BadRequest(ModelState);
            
            service.Create(entity);
            return Created($"api/language/{name}", entity);
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

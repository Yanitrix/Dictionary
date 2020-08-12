using Api.Dto;
using Api.Service;
using AutoMapper;
using Dictionary_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/speechPart")]
    public class SpeechPartController : Controller
    {
        public SpeechPartController(ISpeechPartService service, IMapper mapper)
        {
            this.mapper = mapper;
            this.service = service;
        }

        private readonly ISpeechPartService service;
        private readonly IMapper mapper;

        [HttpGet]
        public IEnumerable<SpeechPart> Index()
        {
            return service.All();
        }

        [HttpPost]
        public IActionResult Create([FromBody] SpeechPartDto dto)
        {
            var entity = mapper.Map(dto);

            service.ValidationDictionary = new ValidationDictionary(ModelState);

            if (!service.IsReadyToAdd(entity)) return BadRequest(service.ValidationDictionary);

            var res = service.Create(entity);

            return Created("", res);
        }

        [HttpPut]
        public IActionResult Update(String language, String name, [FromBody] SpeechPartDto dto)
        {
            var indb = service.GetByNameAndLanguage(language, name);
            if (indb == null) return NotFound();

            var entity = mapper.Map(dto);
            service.ValidationDictionary = new ValidationDictionary(ModelState);
            if (!service.IsReadyToUpdate(entity)) return BadRequest(ModelState);

            var res = service.Update(entity);

            return Ok(res);
        }

        [HttpGet("{index}")]
        public IActionResult Get(int index)
        {
            var entity = service.GetByIndex(index);
            if (entity == null) return NotFound();

            return Json(entity);
        }

        [HttpGet]
        public IActionResult Get(String language, String name)
        {
            var entity = service.GetByNameAndLanguage(language, name);
            if (entity == null) return NotFound();

            return Json(entity);
        }

        [HttpDelete]
        public IActionResult Delete(String language, String name)
        {
            var entity = service.GetByNameAndLanguage(language, name);
            if (entity == null) return NotFound();

            service.Delete(entity);

            return NoContent();
        }
    }
}

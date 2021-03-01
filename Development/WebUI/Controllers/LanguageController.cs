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
        /// <summary>
        /// Retrieves all languages with their word count
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<LanguageWordCount>> Index()
        {
            return service.AllWithWordCount().ToList();
        }

        [HttpGet("{name}")]
        public ActionResult<GetLanguage> Get(String name)
        {
            var found = service.Get(name);

            if (found == null)
                return NotFound($"Language with Name: \"{name}\" doesn't exist");
            return found;
        }

        /// <summary>
        /// Creates a language
        /// </summary>
        /// <response code="201">Language created successfully</response>
        /// <response code="400">Model invalid or related entities not found</response>
        [HttpPost]
        public IActionResult Create([FromBody] CreateLanguage dto)
        {
            var result = service.Add(dto);
            if (!result.IsValid)
                return BadRequest(result);
            var response = ToDto(result.Entity as Language);
            return Created("api/language/" + response.Name, response);
        }

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Entity not found</response>
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

using AutoMapper;
using Data.Dto;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/dictionary")]
    public class DictionaryController : Controller
    {

        private readonly IService<Dictionary> service;
        private readonly IDictionaryRepository repo;
        private IMapper mapper;

        public DictionaryController(IService<Dictionary> service, IDictionaryRepository repo, IMapper mapper)
        {
            this.service = service;
            this.repo = repo;
            this.mapper = mapper;
        }

        //TODO maybe return all with counted entries and expressions?
        //some querying by languages maybe
        [HttpGet]
        public IEnumerable<DictionaryDto> Index(String langIn = null, String langOut = null, String lang = null)
        {
            if ((langIn != null || langOut != null) && lang != null) return Enumerable.Empty<DictionaryDto>();
            if (langIn == null && langOut == null && lang == null) return repo.All().Select(ToDto);
            if (lang != null) return repo.GetAllByLanguage(lang).Select(ToDto);
            if (langIn != null && langOut == null) return repo.GetAllByLanguageIn(langIn).Select(ToDto);
            if (langIn == null && langOut != null) return repo.GetAllByLanguageOut(langOut).Select(ToDto);
            return new DictionaryDto[] { ToDto(repo.GetByLanguageInAndOut(langIn, langOut)) };
        }

        [HttpGet("{index}")]
        public IActionResult Get(int index)
        {
            var entity = repo.GetByIndex(index);
            if (entity == null) return NotFound();
            return Json(ToDto(entity));
        }

        [HttpPost]
        public IActionResult Post([FromBody] DictionaryDto dto)
        {
            var entity = ToDict(dto);
            var result = service.TryAdd(entity);
            if (!result.IsValid) return BadRequest(result);
            return Created("api/dictionary" + entity.Index, entity);
        }

        [HttpDelete("{index}")]
        public IActionResult Delete(int index)
        {
            var entity = repo.GetByIndex(index);
            if (entity == null) return NotFound();
            repo.Delete(entity);
            return NoContent();
        }

        private DictionaryDto ToDto(Dictionary d) => mapper.Map<Dictionary, DictionaryDto>(d);

        private Dictionary ToDict(DictionaryDto dto) => mapper.Map<DictionaryDto, Dictionary>(dto);
    }
}

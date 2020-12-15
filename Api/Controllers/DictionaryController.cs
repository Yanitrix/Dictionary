using Data.Dto;
using Data.Mapper;
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
        public IEnumerable<GetDictionary> Index(String langIn = null, String langOut = null, String lang = null)
        {
            if ((langIn != null || langOut != null) && lang != null) return Enumerable.Empty<GetDictionary>();
            if (langIn == null && langOut == null && lang == null) return repo.All().Select(ToDto);
            if (lang != null) return repo.GetAllByLanguage(lang).Select(ToDto);
            if (langIn != null && langOut == null) return repo.GetAllByLanguageIn(langIn).Select(ToDto);
            if (langIn == null && langOut != null) return repo.GetAllByLanguageOut(langOut).Select(ToDto);
            return new GetDictionary[] { ToDto(repo.GetByLanguageInAndOut(langIn, langOut)) };
        }

        [HttpGet("{index}")]
        public ActionResult<GetDictionary> Get(int index)
        {
            var entity = repo.GetByIndex(index);
            if (entity == null)
                return NotFound();
            return ToDto(entity);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateDictionary dto)
        {
            var entity = ToEntity(dto);
            var result = service.TryAdd(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/dictionary" + entity.Index, ToDto(entity));
        }

        [HttpDelete("{index}")]
        public IActionResult Delete(int index)
        {
            var entity = repo.GetByIndex(index);
            if (entity == null)
                return NotFound();
            repo.Delete(entity);
            return NoContent();
        }

        private GetDictionary ToDto(Dictionary d) => mapper.Map<Dictionary, GetDictionary>(d);
        private Dictionary ToEntity(CreateDictionary dto) => mapper.Map<CreateDictionary, Dictionary>(dto);
    }
}

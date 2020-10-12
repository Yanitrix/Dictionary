using AutoMapper;
using Data.Dto;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/entry")]
    public class EntryController : Controller
    {
        private readonly IService<Entry> service;
        private readonly IEntryRepository repo;
        private readonly IMapper mapper;

        public EntryController(IService<Entry> service, IEntryRepository repo, IMapper mapper)
        {
            this.service = service;
            this.repo = repo;
            this.mapper = mapper;
        }

        //[HttpGet]
        //All()? i dont think so

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entry = repo.GetByID(id);
            if (entry == null) return NotFound();
            return Json(entry);
        }

        [HttpPost]
        public IActionResult Post([FromBody] EntryDto dto)
        {
            var entry = ToEntry(dto);
            var result = service.TryAdd(entry);
            if (!result.IsValid) return BadRequest(result);
            return Created("api/entry/" + entry.ID, entry);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] EntryDto dto)
        {
            var entry = ToEntry(dto);
            entry.ID = id;
            var result = service.TryUpdate(entry);
            if (!result.IsValid) return BadRequest(result);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entry = repo.GetByID(id);
            if (entry == null) return NotFound();
            repo.Delete(entry);
            return NoContent();
        }

        private Entry ToEntry(EntryDto dto) => mapper.Map<EntryDto, Entry>(dto);
        private EntryDto ToDto(Entry e) => mapper.Map<Entry, EntryDto>(e);
    }
}

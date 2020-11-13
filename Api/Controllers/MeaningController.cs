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
    public class MeaningController : Controller
    {
        private readonly IService<Meaning> service;
        private readonly IMeaningRepository repo;
        private readonly IMapper mapper;

        public MeaningController(IService<Meaning> service, IMeaningRepository repo, IMapper mapper)
        {
            this.service = service;
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var meaning = repo.GetByID(id);
            if (meaning == null)
                return NotFound();

            return Json(ToDto(meaning));
        }

        //TODO check if returned http codes are good and also take care of magic strings like "api/meaning"
        [HttpPost]
        public IActionResult Post([FromBody] MeaningDto dto)
        {
            var entity = ToEntity(dto);
            var result = service.TryAdd(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/meaning" + entity.ID, ToDto(entity));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] MeaningDto dto)
        {
            var entity = ToEntity(dto);
            entity.ID = id;
            var result = service.TryUpdate(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = repo.GetByID(id);
            if (entity == null)
                return NotFound();
            repo.Delete(entity);
            return NoContent();
        }

        private MeaningDto ToDto(Meaning m) => mapper.Map<Meaning, MeaningDto>(m);
        private Meaning ToEntity(MeaningDto dto) => mapper.Map<MeaningDto, Meaning>(dto);
    }
}

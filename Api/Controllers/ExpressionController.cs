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
    [Route("api/expression")]
    public class ExpressionController : Controller
    {
        private readonly IService<Expression> service;
        private readonly IExpressionRepository repo;
        private readonly IMapper mapper;

        public ExpressionController(IService<Expression> service, IExpressionRepository repo, IMapper mapper)
        {
            this.service = service;
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet("{int}")]
        public IActionResult Get(int id)
        {
            var exp = repo.GetByID(id);
            if (exp == null) return NotFound();
            return Json(exp);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ExpressionDto dto)
        {
            var exp = ToExp(dto);
            var result = service.TryAdd(exp);
            if (!result.IsValid) return BadRequest(result);
            return Created("api/expression" + exp.ID, exp);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ExpressionDto dto)
        {
            var exp = ToExp(dto);
            exp.ID = id;
            var result = service.TryUpdate(exp);
            if (!result.IsValid) return BadRequest(result);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var exp = repo.GetByID(id);
            if (exp == null) return NotFound();
            repo.Delete(exp);
            return NoContent();
        }

        private Expression ToExp(ExpressionDto dto) => mapper.Map<ExpressionDto, Expression>(dto);
    }
}

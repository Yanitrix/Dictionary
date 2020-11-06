using AutoMapper;
using Data.Dto;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Repository;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/expression")]
    public class FreeExpressionController : Controller
    {
        private readonly IService<FreeExpression> service;
        private readonly IFreeExpressionRepository repo;
        private readonly IMapper mapper;

        public FreeExpressionController(IService<FreeExpression> service, IFreeExpressionRepository repo, IMapper mapper)
        {
            this.service = service;
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var exp = repo.GetByID(id);
            if (exp == null) return NotFound();
            return Json(exp);
        }

        [HttpPost]
        public IActionResult Post([FromBody] FreeExpressionDto dto)
        {
            var exp = ToExp(dto);
            var result = service.TryAdd(exp);
            if (!result.IsValid) return BadRequest(result);
            return Created("api/expression" + exp.ID, exp);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] FreeExpressionDto dto)
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

        private FreeExpression ToExp(FreeExpressionDto dto) => mapper.Map<FreeExpressionDto, FreeExpression>(dto);
    }
}

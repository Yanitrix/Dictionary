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
        public ActionResult<GetFreeExpression> Get(int id)
        {
            var entity = repo.GetByID(id);
            if (entity == null)
                return NotFound();
            return ToDto(entity);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateOrUpdateFreeExpression dto)
        {
            var entity = ToEntity(dto);
            var result = service.TryAdd(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/expression" + entity.ID, ToDto(entity));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateOrUpdateFreeExpression dto)
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

        private FreeExpression ToEntity(CreateOrUpdateFreeExpression dto) => mapper.Map<CreateOrUpdateFreeExpression, FreeExpression>(dto);
        private GetFreeExpression ToDto(FreeExpression exp) => mapper.Map<FreeExpression, GetFreeExpression>(exp);
    }
}

using Domain.Dto;
using Service.Mapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/expression")]
    public class FreeExpressionController : Controller
    {
        private readonly IFreeExpressionService service;
        private readonly IMapper mapper;

        public FreeExpressionController(IFreeExpressionService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<GetFreeExpression> Get(int id)
        {
            var entity = service.Get(id);
            if (entity == null)
                return NotFound();
            return ToDto(entity);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateOrUpdateFreeExpression dto)
        {
            var entity = ToEntity(dto);
            var result = service.Add(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/expression" + entity.ID, ToDto(entity));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateOrUpdateFreeExpression dto)
        {
            var entity = ToEntity(dto);
            entity.ID = id;
            var result = service.Update(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = service.Delete(id);
            if (!result.IsValid)
                return NotFound(result);
            return NoContent();
        }

        private FreeExpression ToEntity(CreateOrUpdateFreeExpression dto) => mapper.Map<CreateOrUpdateFreeExpression, FreeExpression>(dto);
        private GetFreeExpression ToDto(FreeExpression exp) => mapper.Map<FreeExpression, GetFreeExpression>(exp);
    }
}

using Domain.Dto;
using Service.Mapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using static WebUI.Utils.ErrorMessages;

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
            var found = service.Get(id);
            if (found == null)
                return NotFound();
            return found;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateFreeExpression dto)
        {
            var result = service.Add(dto);
            if (!result.IsValid)
                return BadRequest(result);
            var response = ToDto(result.Entity as FreeExpression);
            return Created("api/expression/" + response.ID, response);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateFreeExpression dto)
        {
            if (id != dto.ID)
                return BadRequest(ROUTE_PARAMETER_NOT_MATCH);
            var result = service.Update(dto);
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

        private GetFreeExpression ToDto(FreeExpression exp) => mapper.Map<FreeExpression, GetFreeExpression>(exp);
    }
}

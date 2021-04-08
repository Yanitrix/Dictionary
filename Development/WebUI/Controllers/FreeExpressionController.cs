using Domain.Commands;
using Domain.Dto;
using Domain.Models;
using Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using static WebUI.Utils.ErrorMessages;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/expression")]
    public class FreeExpressionController : Controller
    {
        [HttpGet("{id}")]
        public ActionResult<GetFreeExpression> Get(int id, [FromServices] IQueryHandler<FreeExpressionByIdQuery, GetFreeExpression> handler)
        {
            var query = new FreeExpressionByIdQuery(id);
            return handler.Handle(query) ?? (ActionResult<GetFreeExpression>) NotFound();
        }

        /// <summary>
        /// Creates a free expression
        /// </summary>
        /// <response code="201">Entity created successfully</response>
        /// <response code="400">Model invalid or related entities not found</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateFreeExpressionCommand dto,
            [FromServices] ICommandHandler<CreateFreeExpressionCommand, GetFreeExpression> handler)
        {
            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Created("api/expression/" + response.Entity.ID, response.Entity);
            return BadRequest(dto);
        }

        /// <summary>
        /// Updates a free expression
        /// </summary>
        /// <response code="200">Entity updated successfully</response>
        /// <response code="400">Model invalid or related entities not found</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateFreeExpressionCommand dto,
            [FromServices] ICommandHandler<UpdateFreeExpressionCommand, GetFreeExpression> handler)
        {
            if (id != dto.ID)
                return BadRequest(ROUTE_PARAMETER_NOT_MATCH);
            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Ok();
            return BadRequest(response.Errors);
        }

        /// <summary>
        /// Deletes a free expression with given id
        /// </summary>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] ICommandHandler<DeleteFreeExpressionCommand, FreeExpression> handler)
        {
            var command = new DeleteFreeExpressionCommand
            {
                PrimaryKey = id
            };

            var response = handler.Handle(command);
            if (response.IsSuccessful)
                return NoContent();
            return NotFound(response.Errors);
        }
    }
}

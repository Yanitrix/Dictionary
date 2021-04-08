using Domain.Commands;
using Domain.Dto;
using Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using static WebUI.Utils.ErrorMessages;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/meaning")]
    public class MeaningController : Controller
    {
        [HttpGet("{id}")]
        public ActionResult<GetMeaning> Get(int id, [FromServices] IQueryHandler<MeaningByIdQuery, GetMeaning> handler)
        {
            var query = new MeaningByIdQuery(id);
            return handler.Handle(query) ?? (ActionResult<GetMeaning>) NotFound();
        }

        //TODO check if returned http codes are good and also take care of magic strings like "api/meaning"
        /// <summary>
        /// Created a meaning
        /// </summary>
        /// <response code="201">Meaning created successfully</response>
        /// <response code="400">Model invalid or related entities not found</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateMeaningCommand dto, [FromServices] ICommandHandler<CreateMeaningCommand, GetMeaning> handler)
        {
            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Created("api/meaning/" + response.Entity.ID, response.Entity);
            return BadRequest(response.Errors);
        }
        
        
        /// <summary>
        /// Updates a meaning. Not all values can be updated.
        /// </summary>
        /// <response code="200">Update successful</response>
        /// <response code="400">Model invalid or related entities not found</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateMeaningCommand dto, [FromServices] ICommandHandler<UpdateMeaningCommand, GetMeaning> handler)
        {
            if (id != dto.ID)
                return BadRequest(ROUTE_PARAMETER_NOT_MATCH);
            var response = handler.Handle(dto);
            if (response.IsSuccessful)
                return Ok();
            return BadRequest(response.Errors);
        }

        /// <summary>
        /// Deletes a meaning
        /// </summary>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] DeleteMeaningCommandHandler handler)
        {
            var command = new DeleteMeaningCommand
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

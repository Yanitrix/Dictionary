﻿using Domain.Dto;
using Service.Mapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using static WebUI.Utils.ErrorMessages;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/meaning")]
    public class MeaningController : Controller
    {
        private readonly IMeaningService service;
        private readonly IMapper mapper;

        public MeaningController(IMeaningService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<GetMeaning> Get(int id)
        {
            var found = service.Get(id);
            if (found == null)
                return NotFound();
            return found;
        }

        //TODO check if returned http codes are good and also take care of magic strings like "api/meaning"
        /// <summary>
        /// Created a meaning
        /// </summary>
        /// <response code="201">Meaning created successfully</response>
        /// <response code="400">Model invalid or related entities not found</response>
        [HttpPost]
        public IActionResult Post([FromBody] CreateMeaning dto)
        {
            var result = service.Add(dto);
            if (!result.IsValid)
                return BadRequest(result);
            var response = ToDto(result.Entity as Meaning);
            return Created("api/meaning/" + response.ID, response);
        }
        
        
        /// <summary>
        /// Updates a meaning. Not all values can be updated.
        /// </summary>
        /// <response code="200">Update successful</response>
        /// <response code="400">Model invalid or related entities not found</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateMeaning dto)
        {
            if (id != dto.ID)
                return BadRequest(ROUTE_PARAMETER_NOT_MATCH);
            var result = service.Update(dto);
            if (!result.IsValid)
                return BadRequest(result);
            return Ok();
        }

        /// <summary>
        /// Deletes a meaning
        /// </summary>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = service.Delete(id);
            if (!result.IsValid)
                return NotFound(result);
            return NoContent();
        }

        private GetMeaning ToDto(Meaning m) => mapper.Map<Meaning, GetMeaning>(m);
    }
}

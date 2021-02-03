using Data.Dto;
using Data.Mapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Application.Controllers
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
            var entity = service.Get(id);
            if (entity == null)
                return NotFound();

            return ToDto(entity);
        }

        //TODO check if returned http codes are good and also take care of magic strings like "api/meaning"
        [HttpPost]
        public IActionResult Post([FromBody] CreateMeaning dto)
        {
            var entity = ToEntity(dto);
            var result = service.Add(entity);
            if (!result.IsValid)
                return BadRequest(result);
            return Created("api/meaning" + entity.ID, ToDto(entity));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateMeaning dto)
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

        private GetMeaning ToDto(Meaning m) => mapper.Map<Meaning, GetMeaning>(m);
        private Meaning ToEntity(CreateMeaning dto) => mapper.Map<CreateMeaning, Meaning>(dto);
        private Meaning ToEntity(UpdateMeaning dto) => mapper.Map<UpdateMeaning, Meaning>(dto);
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Entities.Vehicle.Command;
using Parking.API.Application.Entities.Vehicle.Query;

namespace Parking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {

        private readonly IMediator _mediator;

        public VehicleController(IMediator mediator){_mediator = mediator;}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id){           
            var entitie= await _mediator.Send(new VehicleByIdQuery(id));                            
            return Ok(entitie);          
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _mediator.Send(new VehicleQuery());
            if (entities == null)
            {
                return NotFound();
            }
            return Ok(entities);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VehicleDTO entity)
        {
            var savedEntity = await _mediator.Send(new VehicleCreateRequest { Vehicle = entity });
            return Ok(savedEntity);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] VehicleResponseDTO entity, Guid id)
        {
            var savedEntity = await _mediator.Send(new VehicleUpdateRequest { Vehicle = entity, Id = id}   );
            return Ok(savedEntity);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {           
            try
            {
               await _mediator.Send(new VehicleDeleteRequest { Id = id });
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }           
        }

    }
}

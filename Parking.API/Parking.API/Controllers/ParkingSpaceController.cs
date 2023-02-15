using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.Entities.ParkingSpaces.Command;
using Parking.API.Application.Entities.ParkingSpaces.Query;


namespace Parking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpaceController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ParkingSpaceController(IMediator mediator) { _mediator = mediator; }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entitie = await _mediator.Send(new ParkingSpaceByIdRequest { Id = id });
            return Ok(entitie);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _mediator.Send(new ParkingSpaceRequest());
            if (entities == null)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ParkingSpaceDTO entityDto)
        {
            var savedEntity = await _mediator.Send(new ParkingSpaceCreateRequest { ParkingSpace = entityDto });
            return Ok(savedEntity);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ParkingSpaceResponseDTO entityDto )
        {
            var savedEntity = await _mediator.Send(new ParkingSpaceUpdateRequest { ParkingSpace = entityDto,Id= id });
            return Ok(savedEntity);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new ParkingSpaceDeleteRequest { Id = id });
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}

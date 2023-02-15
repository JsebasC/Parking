using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Entities.Parking.Command;
using Parking.API.Application.Entities.Parking.Query;

namespace Parking.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]        
    public class ParkingController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ParkingController(IMediator mediator){_mediator = mediator;}

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id){           
            var entitie= await _mediator.Send(new ParkingByIdRequest { Id = id });                            
            return Ok(entitie);          
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _mediator.Send(new ParkingRequest());
            if (entities == null)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpGet("GetAllNotExitVehicle")]
        public async Task<IActionResult> GetAllExitVehicle()
        {
            var entities = await _mediator.Send(new ParkingExitRequest());
            if (entities == null)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpGet("GetValuePayExitVehicle/{plate}")]
        public async Task<IActionResult> GetValuePayExitVehicle(string plate)
        {
            var entities = await _mediator.Send(new ParkingValuePayRequest { Plate = plate});
            if (entities == null)
            {
                return NotFound();
            }

            return Ok(entities);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ParkingDTO entityDto)
        {
            var savedEntity = await _mediator.Send(new ParkingCreateRequest { Parking = entityDto });
            return Ok(savedEntity);
        }

        
        [HttpPut("ExitVehicle/{id}")]
        public async Task<IActionResult> Put(Guid id,[FromBody] ParkingUpdateDTO entityDto)
        {
            var savedEntity = await _mediator.Send(new ParkingUpdateRequest { Parking = entityDto,Id = id });
            return Ok(savedEntity);
        }


        [EnableCors]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {           
            try
            {
               await _mediator.Send(new ParkingDeleteRequest { Id = id });
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }           
        }

    }
}

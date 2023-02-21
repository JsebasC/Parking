using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.Entities.ParkingSpaces.Command;
using Parking.API.Application.Entities.ParkingSpaces.Query;
using Swashbuckle.AspNetCore.Annotations;

namespace Parking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpaceController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ParkingSpaceController(IMediator mediator) { _mediator = mediator; }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtener información del bloque del parqueadero por Id", Description = "Esto se utiliza para obtener la información de los vehiculos ocupados o no en el parqueadero")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entitie = await _mediator.Send(new ParkingSpaceByIdRequest { Id = id });
            return Ok(entitie);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Obtener toda la información del bloque del parqueadero", Description = "")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _mediator.Send(new ParkingSpaceRequest());
            return Ok(entities);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Agregar bloques al parqueadero", Description = "")]
        public async Task<IActionResult> Post([FromBody] ParkingSpaceDTO entityDto)
        {
            var savedEntity = await _mediator.Send(new ParkingSpaceCreateRequest { ParkingSpace = entityDto });
            return Ok(savedEntity);
        }


        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Actualizar la información del bloque del parqueadero", Description = "")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ParkingSpaceResponseDTO entityDto )
        {
            var savedEntity = await _mediator.Send(new ParkingSpaceUpdateRequest { ParkingSpace = entityDto,Id= id });
            return Ok(savedEntity);
        }


        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Eliminar un espacio del parqueadero", Description = "")]
        public async Task<IActionResult> Delete(Guid id)
        {            
                await _mediator.Send(new ParkingSpaceDeleteRequest { Id = id });
                return Ok();         
        }
    }
}

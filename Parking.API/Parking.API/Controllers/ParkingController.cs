using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Entities.Parking.Command;
using Parking.API.Application.Entities.Parking.Query;
using Swashbuckle.AspNetCore.Annotations;

namespace Parking.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]        
    public class ParkingController : ControllerBase
    {

        private readonly IMediator _mediator;
        public ParkingController(IMediator mediator){_mediator = mediator;}

       
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtener información del parqueo ById", Description = "")]
        public async Task<IActionResult> GetById(Guid id){           
            var entitie= await _mediator.Send(new ParkingByIdRequest { Id = id });                            
            return Ok(entitie);          
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Obtener todos los vehiculos parqueados", Description = "")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _mediator.Send(new ParkingRequest());
            return Ok(entities);
        }

        [HttpGet("GetAllNotExitVehicle")]
        [SwaggerOperation(Summary = "Obtener todos los vehiculos que no han salido del parqueo", Description = "")]
        public async Task<IActionResult> GetAllExitVehicle()
        {
            var entities = await _mediator.Send(new ParkingExitRequest());   
            return Ok(entities);
        }

        [HttpGet("GetValuePayExitVehicle/{plate}")]
        [SwaggerOperation(Summary = "Obtener el valor a pagar de un vehiculo cuando va a salir", Description = "")]
        public async Task<IActionResult> GetValuePayExitVehicle(string plate)
        {
            var entities = await _mediator.Send(new ParkingValuePayRequest { Plate = plate});     
            return Ok(entities);
        }
       
        [HttpPost("ParkingVehicle")]
        [SwaggerOperation(Summary = "Agregar vehiculo By Placa para parquear", Description = "Este metodo agrega el vehiculo si no existe o existente y registra el vehiculo")]
        public async Task<IActionResult> ParkingVehicle([FromBody] ParkingVehicleDTO entityDto)
        {            
            return Ok(await _mediator.Send(new ParkingVehicleRequest { ParkingVehicle = entityDto }));
        }

        [HttpPut("ExitVehicle/{id}")]
        [SwaggerOperation(Summary = "Salida a los vehiculos dentro del parqueadero", Description = "Este metodo m ayuda a darle salida a un vehiculo por el ID del Parqueadero")]
        public async Task<IActionResult> Put(Guid id,[FromBody] ParkingUpdateDTO entityDto)
        {
            var savedEntity = await _mediator.Send(new ParkingUpdateRequest { Parking = entityDto,Id = id });
            return Ok(savedEntity);
        }
    }
}

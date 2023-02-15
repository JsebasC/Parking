using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.Entities.Parking.Command;
using Parking.API.Application.Entities.Vehicle.Command;
using Parking.API.Application.Utils;
using Parking.API.Infrastructure.Data;

namespace Parking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingVehicleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ParkingContext _context;
        public ParkingVehicleController(IMediator mediator, ParkingContext parkingContext) { _mediator = mediator; _context = parkingContext; }

        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ParkingVehicleDTO entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    VehicleDTO vehicleDTO = new VehicleDTO();
                    vehicleDTO.Plate = entity.Plate;
                    vehicleDTO.CubicCentimeters = entity.CubicCentimeters;
                    vehicleDTO.VehicleType = entity.VehicleType;

                    Utils.PlateOddEvenDay(vehicleDTO.Plate, vehicleDTO.VehicleType, Utils.OddEvenDay());

                    var savedEntity = await _mediator.Send(new VehicleCreateRequest { Vehicle = vehicleDTO });
                    
                    ParkingDTO parkingSpaceDTO = new ParkingDTO();
                    parkingSpaceDTO.VehicleID = savedEntity.Id;
                    parkingSpaceDTO.ParkingSpacesID =new Guid(entity.idParkingSpace);
                    var parking = await _mediator.Send(new ParkingCreateRequest { Parking = parkingSpaceDTO });
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return BadRequest(e.Message);
                    
                }
            }           
            
        }


    }
}

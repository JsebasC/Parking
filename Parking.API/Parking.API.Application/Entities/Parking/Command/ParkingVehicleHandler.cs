using AutoMapper;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Domain.Services;

namespace Parking.API.Application.Entities.Parking.Command
{

    public record ParkingVehicleRequest : IRequest<ParkingVehicleDTO>
    {
        public ParkingVehicleDTO? ParkingVehicle { get; set; }
    }
    public class ParkingVehicleHandler : IRequestHandler<ParkingVehicleRequest, ParkingVehicleDTO>
    {
        readonly VehicleParkingService _parkingService;
        readonly IMapper _mapper;

        public ParkingVehicleHandler(VehicleParkingService ParkingService, IMapper mapper)
        {
            _parkingService = ParkingService;
            _mapper = mapper;
        }

        public async Task<ParkingVehicleDTO> Handle(ParkingVehicleRequest request, CancellationToken cancellationToken)
        {          
            await _parkingService.InsertParkingVehicle(new Domain.Entities.Vehicle() { Plate = 
                request.ParkingVehicle!.Plate, CubicCentimeters = request.ParkingVehicle.CubicCentimeters, 
                VehicleType = request.ParkingVehicle.VehicleType }, new Guid(request.ParkingVehicle.idParkingSpace!));

            return await Task.FromResult(_mapper.Map<ParkingVehicleDTO>(request.ParkingVehicle));
        }
    }
}

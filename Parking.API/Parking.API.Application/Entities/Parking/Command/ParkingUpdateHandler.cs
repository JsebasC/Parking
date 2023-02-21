using AutoMapper;
using MediatR;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Domain.Services;

namespace Parking.API.Application.Entities.Parking.Command
{

    public record ParkingUpdateRequest : IRequest<ParkingResponseDTO>
    {
        public Guid Id { get; set; }
        public ParkingUpdateDTO? Parking { get; set; }
    }

    public class ParkingUpdateHandler : IRequestHandler<ParkingUpdateRequest, ParkingResponseDTO>
    {
        readonly ParkingService _parkingService;
        private readonly IMapper _mapper;

        public ParkingUpdateHandler(ParkingService parkingService, IMapper mapper)
        {
            _parkingService = parkingService;
            _mapper = mapper;           
        }

        public async Task<ParkingResponseDTO> Handle(ParkingUpdateRequest request, CancellationToken cancellationToken)
        {
           
            var entity = _mapper.Map<Domain.Entities.Parking>(request.Parking);            
            if (entity.ExitDate ==null)
                entity.ExitDate = DateTime.Now;    
            
            await _parkingService.PayParkingVehicle(request.Id,entity);            
            return await Task.FromResult(_mapper.Map<ParkingResponseDTO>(entity));
        }
    }
}


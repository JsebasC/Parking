using AutoMapper;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.Entities.ParkingSpaces.Command;
using Parking.API.Domain.DataAccess;

namespace Parking.API.Application.Entities.ParkingVehicle.Command
{
    public record ParkingVehicleCreateRequest : IRequest<ParkingVehicleDTO>
    {
        public ParkingVehicleDTO ParkingVehicle { get; set; }
    }
    public class ParkingVehicleCreateHandler : IRequestHandler<ParkingVehicleCreateRequest, ParkingVehicleDTO>
    {

        private readonly IRepository<Domain.Entities.Parking> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ParkingVehicleCreateHandler(IRepository<Domain.Entities.Parking> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ParkingVehicleDTO> Handle(ParkingVehicleCreateRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Parking>(request.ParkingVehicle);

            await _repository.InsertTransaction(entity);


            return await Task.FromResult(_mapper.Map<ParkingVehicleDTO>(entity));

        }
    }
}

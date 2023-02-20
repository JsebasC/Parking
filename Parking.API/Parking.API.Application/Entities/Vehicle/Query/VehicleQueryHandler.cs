using AutoMapper;
using MediatR;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.Ports;

namespace Parking.API.Application.Entities.Vehicle.Query
{
    public class ParkingRateQueryHandler : IRequestHandler<VehicleQuery, List<VehicleResponseDTO>>
    {
        private readonly IRepository<Domain.Entities.Vehicle> _repository;
        private readonly IMapper _mapper;

        public ParkingRateQueryHandler(IRepository<Domain.Entities.Vehicle> repository, IMapper mapper)
        {            
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<List<VehicleResponseDTO>> Handle(VehicleQuery request, CancellationToken cancellationToken)
        {
            var entities = _mapper.Map<List<VehicleResponseDTO>>(await _repository.GetAll());
            if (entities == null)
                throw new NotFoundException(nameof(request));
            return await Task.FromResult(entities);
        }
    }
}

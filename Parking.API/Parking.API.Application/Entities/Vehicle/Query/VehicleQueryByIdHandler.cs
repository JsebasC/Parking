using AutoMapper;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.Ports;

namespace Parking.API.Application.Entities.Vehicle.Query
{   
    public class VehicleQueryByIdHandler : IRequestHandler<VehicleByIdQuery, VehicleResponseDTO>
    {
        private readonly IRepository<Domain.Entities.Vehicle> _repository;
        private readonly IMapper _mapper;

        public VehicleQueryByIdHandler(IRepository<Domain.Entities.Vehicle> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<VehicleResponseDTO> Handle(VehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id);
            if (entity == null)
                throw new NotFoundException(nameof(request), request.Id);

            return await Task.FromResult(_mapper.Map<VehicleResponseDTO>(entity));
        }
    }
}

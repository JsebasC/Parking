using AutoMapper;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.Ports;

namespace Parking.API.Application.Entities.ParkingSpaces.Query
{
    public class ParkingSpaceByIdRequest : IRequest<ParkingSpaceResponseDTO>
    {
        public Guid Id { get; set; }
    }
    public class ParkingSpaceQueryByIdHandler : IRequestHandler<ParkingSpaceByIdRequest, ParkingSpaceResponseDTO>
    {
        private readonly IRepository<Domain.Entities.ParkingSpaces> _repository;
        private readonly IMapper _mapper;

        public ParkingSpaceQueryByIdHandler(IRepository<Domain.Entities.ParkingSpaces> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ParkingSpaceResponseDTO> Handle(ParkingSpaceByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id);

            if (entity == null)
                throw new NotFoundException(nameof(request), request.Id);

            return await Task.FromResult(_mapper.Map<ParkingSpaceResponseDTO>(entity));
        }
    }
}

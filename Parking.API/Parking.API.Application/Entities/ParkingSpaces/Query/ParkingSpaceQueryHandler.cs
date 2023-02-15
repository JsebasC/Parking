using AutoMapper;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.DataAccess;

namespace Parking.API.Application.Entities.ParkingSpaces.Query
{
    public class ParkingSpaceRequest : IRequest<List<ParkingSpaceResponseDTO>> { }

    public class ParkingRateQueryHandler : IRequestHandler<ParkingSpaceRequest, List<ParkingSpaceResponseDTO>>
    {
        private readonly IRepository<Domain.Entities.ParkingSpaces> _repository;
        private readonly IMapper _mapper;

        public ParkingRateQueryHandler(IRepository<Domain.Entities.ParkingSpaces> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<ParkingSpaceResponseDTO>> Handle(ParkingSpaceRequest request, CancellationToken cancellationToken)
        {
            var entities = _mapper.Map<List<ParkingSpaceResponseDTO>>(await _repository.GetAll());

            if (entities == null)
                throw new NotFoundException(nameof(request));


            return await Task.FromResult(entities);

        }
    }


}

using AutoMapper;
using FluentValidation;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Domain.DataAccess;

namespace Parking.API.Application.Entities.ParkingSpaces.Command
{

    public record ParkingSpaceCreateRequest : IRequest<ParkingSpaceDTO>
    {
        public ParkingSpaceDTO ParkingSpace { get; set; }
    }

    public class ParkingRateCreateHandler : IRequestHandler<ParkingSpaceCreateRequest, ParkingSpaceDTO>
    {
        private readonly IRepository<Domain.Entities.ParkingSpaces> _repository;
        private readonly IMapper _mapper;
        public ParkingRateCreateHandler(IRepository<Domain.Entities.ParkingSpaces> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ParkingSpaceDTO> Handle(ParkingSpaceCreateRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.ParkingSpaces>(request.ParkingSpace);
            await _repository.Insert(entity);
            return await Task.FromResult(_mapper.Map<ParkingSpaceDTO>(entity));
        }
    }

    public class ParkingSpaceCreateValidator : AbstractValidator<ParkingSpaceCreateRequest>
    {
        public ParkingSpaceCreateValidator()
        {
            RuleFor(r => r.ParkingSpace.Name).NotEmpty().WithMessage("La placa no debe estar vacia");
            RuleFor(r => r.ParkingSpace.Space).NotEmpty().WithMessage($"El espacio se debe ingresar");
        }
    }

}

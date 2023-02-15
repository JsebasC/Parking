using AutoMapper;
using FluentValidation;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.DataAccess;

namespace Parking.API.Application.Entities.ParkingSpaces.Command
{

    public record ParkingSpaceUpdateRequest : IRequest<ParkingSpaceResponseDTO>
    {
        public Guid Id { get; set; }
        public ParkingSpaceResponseDTO ParkingSpace { get; set; }
    }

    public class ParkingSpaceUpdateHandler : IRequestHandler<ParkingSpaceUpdateRequest, ParkingSpaceResponseDTO>
    {
        private readonly IRepository<Domain.Entities.ParkingSpaces> _repository;
        private readonly IMapper _mapper;
        public ParkingSpaceUpdateHandler(IRepository<Domain.Entities.ParkingSpaces> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ParkingSpaceResponseDTO> Handle(ParkingSpaceUpdateRequest request, CancellationToken cancellationToken)
        {
            
            if (request.Id != request.ParkingSpace.Id)
                throw new NotFoundException(nameof(request),request.Id);

            var entity = _mapper.Map<Domain.Entities.ParkingSpaces>(request.ParkingSpace);      
            
            await _repository.Update(entity);
            return await Task.FromResult(_mapper.Map<ParkingSpaceResponseDTO>(entity));
        }
    }

    public class ParkingSpaceUpdateValidator : AbstractValidator<ParkingSpaceUpdateRequest>
    {
        public ParkingSpaceUpdateValidator()
        {
            RuleFor(r => r.ParkingSpace.Name).NotEmpty().WithMessage("La placa no debe estar vacia");
            RuleFor(r => r.ParkingSpace.Space).NotEmpty().WithMessage($"El espacio se debe ingresar");            
        }
    }
}

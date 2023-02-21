using AutoMapper;
using FluentValidation;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Domain.Ports;
using System.Data;

namespace Parking.API.Application.Entities.Vehicle.Command
{

    public record VehicleCreateRequest : IRequest<VehicleResponseDTO>
    {
        public VehicleDTO? Vehicle { get; set; }
    }

    public class ParkingRateCreateHandler : IRequestHandler<VehicleCreateRequest, VehicleResponseDTO>
    {
        private readonly IRepository<Domain.Entities.Vehicle> _repository;
        private readonly IMapper _mapper;
        private readonly IDbConnection _dapperSource;

        public ParkingRateCreateHandler(IRepository<Domain.Entities.Vehicle> repository, IMapper mapper, IDbConnection dapperSource)
        {
            _repository = repository;
            _mapper = mapper;
            _dapperSource = dapperSource;
        }

        public async Task<VehicleResponseDTO> Handle(VehicleCreateRequest request, CancellationToken cancellationToken)
        {            
            
            //var entityFind = await _repository.GetAsync(e => e.Plate == entity.Plate, orderBy: q => q.OrderBy(e => e.Id), includeStringProperties: "");
            var entityFind = await _repository.GetAsync(e => e.Plate == request.Vehicle!.Plate);
            if (entityFind != null)
                throw new Validation.Exceptions.LogicException("Ya existe el vehiculo en el parqueadero");

            var entity = _mapper.Map<Domain.Entities.Vehicle>(request.Vehicle);
            await _repository.Insert(entity);
            return await Task.FromResult(_mapper.Map<VehicleResponseDTO>(entity));
        }
    }

    public class VehicleCreateValidator : AbstractValidator<VehicleCreateRequest>
    {
        public VehicleCreateValidator()
        {
            RuleFor(r => r.Vehicle!.Plate).NotEmpty().WithMessage("La placa no debe estar vacia");
        }
    }

}

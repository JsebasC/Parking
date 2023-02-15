using AutoMapper;
using Dapper;
using FluentValidation;
using MediatR;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.DataAccess;
using System.Data;

namespace Parking.API.Application.Entities.Vehicle.Command
{

    public record VehicleUpdateRequest : IRequest<VehicleResponseDTO>
    {
        public Guid Id { get; set; }
        public VehicleResponseDTO Vehicle { get; set; }
    }

    public class VehicleUpdateHandler : IRequestHandler<VehicleUpdateRequest, VehicleResponseDTO>
    {
        private readonly IRepository<Domain.Entities.Vehicle> _repository;        
        private readonly IMapper _mapper;
        private readonly IDbConnection _dapperSource;
        public VehicleUpdateHandler(IRepository<Domain.Entities.Vehicle> repository, IMapper mapper,IDbConnection dapperSource)
        {
            _repository = repository;
            _mapper = mapper;
            _dapperSource = dapperSource; 
        }

        public async Task<VehicleResponseDTO> Handle(VehicleUpdateRequest request, CancellationToken cancellationToken)
        {
            var existVehicle = _dapperSource.QueryFirstOrDefault<Domain.Entities.Vehicle>("select top 1 * from dbo.Vehicle where Id = @Id  ", new { Id = request.Id });
            if (existVehicle == null)
                throw new Validation.Exceptions.LogicException($"El vehiculo no existe");

            if (request.Id != request.Vehicle.Id)
                throw new NotFoundException(nameof(request),request.Id);

            var entity = _mapper.Map<Domain.Entities.Vehicle>(request.Vehicle);
            await _repository.Update(entity);
            return await Task.FromResult(_mapper.Map<VehicleResponseDTO>(entity));
        }
    }

    public class VehicleUpdateValidator : AbstractValidator<VehicleUpdateRequest>
    {
        public VehicleUpdateValidator()
        {
            RuleFor(r => r.Vehicle.Plate).NotEmpty().WithMessage("La placa no debe estar vacia").Length(6,6).WithMessage("La placa debe contener 6 caracteres");
          
        }
    }
}

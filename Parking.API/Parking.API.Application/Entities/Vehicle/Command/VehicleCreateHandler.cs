using AutoMapper;
using Dapper;
using FluentValidation;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Entities.Parking.Command;
using Parking.API.Domain.Ports;
using System.Data;
using static Dapper.SqlMapper;

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
            var entity = _mapper.Map<Domain.Entities.Vehicle>(request.Vehicle);

            var existVehicleSpace = _dapperSource.QueryFirstOrDefault<Domain.Entities.Vehicle>("select top 1 v.* from dbo.Vehicle v  " +
                "JOIN dbo.Parking p ON v.Id = p.VehicleID where v.Plate = @Plate AND ExitDate IS NULL", new { Plate = entity.Plate });
            if (existVehicleSpace != null)
                throw new Validation.Exceptions.LogicException("Ya existe el vehiculo en el parqueadero");

            var existVehicle = _dapperSource.QueryFirstOrDefault<Domain.Entities.Vehicle>("select top 1 v.* from dbo.Vehicle v where v.Plate = @Plate ", new { Plate = entity.Plate });
             if (existVehicle != null)
                return await Task.FromResult(_mapper.Map<VehicleResponseDTO>(existVehicle));
            
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

using AutoMapper;
using Dapper;
using FluentValidation;
using MediatR;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.DataAccess;
using System.Data;
using static Dapper.SqlMapper;

namespace Parking.API.Application.Entities.Parking.Command
{

    public record ParkingUpdateRequest : IRequest<ParkingResponseDTO>
    {
        public Guid Id { get; set; }
        public ParkingUpdateDTO Parking { get; set; }
    }

    public class ParkingUpdateHandler : IRequestHandler<ParkingUpdateRequest, ParkingResponseDTO>
    {
        private readonly IRepository<Domain.Entities.Parking> _repository;
        private readonly IMapper _mapper;

        private readonly IDbConnection _dapperSource;
        public ParkingUpdateHandler(IRepository<Domain.Entities.Parking> repository, IMapper mapper, IDbConnection dapperSource)
        {
            _repository = repository;
            _mapper = mapper;
            _dapperSource = dapperSource ?? throw new ArgumentNullException(nameof(dapperSource));
        }

        public async Task<ParkingResponseDTO> Handle(ParkingUpdateRequest request, CancellationToken cancellationToken)
        {

            var parking = _dapperSource.QuerySingleOrDefault<Domain.Entities.Parking>("select * from dbo.Parking where Id = @Id ", new { Id = request.Id });
            if (parking == null || request.Id != request.Parking.Id)
                throw new NotFoundException(nameof(request), request.Id);
            if(parking.ExitDate!= null)
                throw new LogicException("Ya se le dio salida a este vehiculo");

            var entity = _mapper.Map<Domain.Entities.Parking>(request.Parking);
            entity.VehicleID = parking.VehicleID;
            entity.ParkingSpacesID = parking.ParkingSpacesID;
            entity.EntryDate = parking.EntryDate;

            if (entity.ExitDate ==null)
            {
                entity.ExitDate = DateTime.Now;
            }
            
            LogicValidationParking logicValidationParking = new LogicValidationParking(_dapperSource);
            ExitValidation(entity.EntryDate, entity.ExitDate, entity);
            
            var getDifferenceExit = Utils.Utils.GetDifferenceExit(entity.EntryDate, entity.ExitDate);

            entity.Second = (long)getDifferenceExit.TotalSeconds;
            entity.TotalValue = logicValidationParking.CalculateValueTotal(entity.EntryDate, entity.ExitDate, getDifferenceExit, entity.VehicleID);

            await _repository.Update(entity);
            logicValidationParking.ManageSpace(parking.ParkingSpacesID, false); //aca salio

            return await Task.FromResult(_mapper.Map<ParkingResponseDTO>(entity));
        }

        #region Validation

      
        /// <summary>
        /// Validar la salida
        /// </summary>
        /// <param name="entryDate"></param>
        /// <param name="exitDate"></param>
        /// <param name="entity"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ExitValidation(DateTime entryDate, DateTime? exitDate, Domain.Entities.Parking entity)
        {
            if (entryDate > exitDate)
                throw new Validation.Exceptions.LogicException($"La fecha de entrada no puede ser mayor a la de salida");

        }

        #endregion
    }


    public class ParkingUpdateValidator : AbstractValidator<ParkingUpdateRequest>
    {
        public ParkingUpdateValidator()
        {
            //RuleFor(r => r.Parking.ExitDate).NotEmpty().WithMessage("La fecha de salida es obligatoria");            
        }
    }
}


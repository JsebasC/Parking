using AutoMapper;
using Dapper;
using FluentValidation;
using MediatR;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.DataAccess;
using System.Data;

namespace Parking.API.Application.Entities.Parking.Command
{

    public record ParkingCreateRequest : IRequest<ParkingDTO>
    {
        public ParkingDTO Parking { get; set; }
    }

    public class ParkingCreateHandler : IRequestHandler<ParkingCreateRequest, ParkingDTO>
    {
        private readonly IRepository<Domain.Entities.Parking> _repository;        
        private readonly IMapper _mapper;
        private readonly IDbConnection _dapperSource;

        public ParkingCreateHandler(IRepository<Domain.Entities.Parking> repository, IMapper mapper,IDbConnection dapperSource)
        {
            _repository = repository;
            //_repositorySpace = repositorySpace;
            _dapperSource = dapperSource;
            _mapper = mapper;
        }

        public async Task<ParkingDTO> Handle(ParkingCreateRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Parking>(request.Parking);
            entity.EntryDate = DateTime.Now;

            //var entityVehicle = _dapperSource.QuerySingleOrDefault<Domain.Entities.Vehicle>("select * from dbo.Vehicle where Id = @Id ", new { Id = request.Parking.VehicleID });
            //if (entityVehicle ==null)
            //    throw new NotFoundException("N0 existe el vehiculo : " + request.Parking.VehicleID);
            
            //PlateOddEvenDay(entityVehicle.Plate, entityVehicle.VehicleType, Utils.Utils.OddEvenDay());
            //GetEntitySpaceValidation(entity.ParkingSpacesID, entityVehicle.VehicleType, request.Parking.VehicleID);

            await _repository.Insert(entity);
            return await Task.FromResult(_mapper.Map<ParkingDTO>(entity));
        }
        #region MyRegion

        
        //Validar spacio disponible
        public void GetEntitySpaceValidation(Guid idParking,int typeVehicle, Guid idVehicle)
        {         
            LogicValidationParking logicValidationParking = new LogicValidationParking(_dapperSource);
            ExitParkingVehicle(idVehicle);
            logicValidationParking.ManageSpace(idParking, typeVehicle, true);    //aca entro      
        }

        /// <summary>
        /// Validar si el vehiculo no ha salido
        /// </summary>
        /// <param name="idVehicle"></param>
        /// <exception cref="Validation.Exceptions.LogicException"></exception>
        public void ExitParkingVehicle(Guid idVehicle)
        {
            var existVehicle = _dapperSource.QueryFirstOrDefault<Domain.Entities.Parking>("select top 1 * from dbo.Parking where VehicleID = @VehicleID and ExitDate IS NULL ", new { VehicleID = idVehicle });
            if (existVehicle != null)
                throw new Validation.Exceptions.LogicException($"El vehiculo no ha salido");
        }

        ///// <summary>
        ///// Validar si el vehiculo entra
        ///// </summary>
        ///// <param name="plate"></param>
        ///// <param name="vehicleType"></param>
        ///// <param name="FlagTodayOddEvenDay"></param>
        ///// <exception cref="Validation.Exceptions.LogicException"></exception>
        //public void PlateOddEvenDay(string plate, int vehicleType, bool FlagTodayOddEvenDay)
        //{
        //    int numero = Utils.Utils.GetNumberPlate(plate, vehicleType);
        //    string mensaje=string.Empty;
        //    string dateString = DateTime.Now.ToString("D");
        //    if ((numero % 2 == 0) && !FlagTodayOddEvenDay)
        //    {
        //        mensaje += "el vehiculo no puede entrar hoy,";
        //        mensaje += string.Format("El numero es par y hoy entran los impares segun el dia de hoy : {0} ", dateString);
        //        throw new Validation.Exceptions.LogicException(mensaje);
        //    }

        //    if (!(numero % 2 == 0) && FlagTodayOddEvenDay)
        //    {
        //        mensaje += "el vehiculo no puede entrar hoy,";
        //        mensaje += string.Format("El numero es impar y hoy entran los pares segun el dia de hoy : {0}" , dateString);
        //        throw new Validation.Exceptions.LogicException(mensaje);
        //    }
        //}


        #endregion
    }

    public class ParkingCreateValidator : AbstractValidator<ParkingCreateRequest>
    {
        public ParkingCreateValidator()
        {
            //RuleFor(r => r.Parking.EntryDate).NotEmpty().WithMessage("La fecha de entrada es obligatoria");
            RuleFor(r => r.Parking.VehicleID).NotEmpty().WithMessage("El vehiculo es obligatorio");
            RuleFor(r => r.Parking.ParkingSpacesID).NotEmpty().WithMessage("El espacio ocupado es obligatorio");            
        }
    }


   

}

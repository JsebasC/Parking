using AutoMapper;
using Dapper;
using MediatR;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Entities.Parking.Command;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.Ports;
using System;
using System.Data;
using static Dapper.SqlMapper;

namespace Parking.API.Application.Entities.Parking.Query
{

    public class ParkingValuePayRequest : IRequest<ParkingResponseDTO>
    {
        public string Plate { get; set; } = string.Empty;
    }
    public class ParkingValuePayQueryHandler : IRequestHandler<ParkingValuePayRequest, ParkingResponseDTO>
    {

        private readonly IDbConnection _dapperSource;
        private readonly IParkingRepository<Domain.Entities.Parking> _parkingRepository;
        private readonly IMapper  _mapper;

        public ParkingValuePayQueryHandler(IDbConnection dapperSource, IParkingRepository<Domain.Entities.Parking> parkingRepository,IMapper mapper)
        {
            _dapperSource = dapperSource ?? throw new ArgumentNullException(nameof(dapperSource));
            _parkingRepository = parkingRepository ?? throw new ArgumentNullException(nameof(parkingRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<ParkingResponseDTO> Handle(ParkingValuePayRequest request, CancellationToken cancellationToken)
        {
            //PROBAR CON JMTER
            //Domain.Entities.Parking entities = await _parkingRepository.GetAllPayVehicle(request.Plate);

            var sql = $"SELECT p.Id Id, v.Id VehicleID,v.Plate Vehicle, ps.Id ParkingSpacesID, ps.Name ParkingSpaces,EntryDate,ExitDate,Second,TotalValue,v.CubicCentimeters FROM dbo.Parking p " +
            $"JOIN dbo.Vehicle v With(nolock) ON p.VehicleID = v.id " +
            $"JOIN dbo.ParkingSpaces ps With(nolock) ON p.ParkingSpacesID = ps.Id where ExitDate IS NULL and v.Plate = @Plate";
            var entities = await _dapperSource.QuerySingleAsync<ParkingResponseDTO>(sql, new{ Plate = request.Plate });

            if (entities == null)
                throw new LogicException("No existe ningun vehiculo dentro del parqueadero con esa placa ");
                        
            //PROBAR CON JMTER
            // Random random = new Random();
            //entities.ExitDate = entities.EntryDate.AddDays(random.Next(1, 365)).AddHours(random.Next(1,24)).AddMinutes(random.Next(1,60));           
            entities.ExitDate = DateTime.Now;

            LogicValidationParking logicValidationParking = new LogicValidationParking(_dapperSource);
            Utils.Utils.ExitValidation(entities.EntryDate, entities.ExitDate);
            var getDifferenceExit = Utils.Utils.GetDifferenceExit(entities.EntryDate, entities.ExitDate);
            entities.Second = (long)getDifferenceExit.TotalSeconds;
            entities.TotalValue = logicValidationParking.CalculateValueTotal(entities.EntryDate, entities.ExitDate, getDifferenceExit, entities.VehicleID);

            //PROBAR CON JMTER
            //logicValidationParking.ManageSpace(entities.ParkingSpacesID, false); //aca salio

            //PROBAR CON JMTER
            //return await Task.FromResult(_mapper.Map<ParkingResponseDTO>(entities));
            return await Task.FromResult(entities);
        }



    }
}

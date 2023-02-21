using Dapper;
using MediatR;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Validation.Exceptions;
using System.Data;

namespace Parking.API.Application.Entities.Parking.Query
{

    public class ParkingExitRequest : IRequest<List<ParkingResponseDTO>> { }
    public class ParkingExitQueryHandler : IRequestHandler<ParkingExitRequest, List<ParkingResponseDTO>>
    {
        private readonly IDbConnection _dapperSource;

        public ParkingExitQueryHandler(IDbConnection dapperSource)
        {
            _dapperSource = dapperSource ?? throw new ArgumentNullException(nameof(dapperSource));
        }

        public async Task<List<ParkingResponseDTO>> Handle(ParkingExitRequest request, CancellationToken cancellationToken)
        {
            var sql = $"SELECT p.Id Id, v.Id VehicleID,v.Plate Vehicle, ps.Id ParkingSpacesID, ps.Name ParkingSpaces,EntryDate,ExitDate,Second,TotalValue,v.VehicleType FROM dbo.Parking p " +
            $"JOIN dbo.Vehicle v With(nolock) ON p.VehicleID = v.id " +
            $"JOIN dbo.ParkingSpaces ps With(nolock) ON p.ParkingSpacesID = ps.Id where ExitDate IS NULL";
            

            var entities = await _dapperSource.QueryAsync<ParkingResponseDTO>(sql);
            if (entities == null)
                throw new NotFoundException(nameof(request));
            return await Task.FromResult(entities.ToList());
        }

    }

}

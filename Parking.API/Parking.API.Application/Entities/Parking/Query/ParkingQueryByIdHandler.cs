using Dapper;
using MediatR;
using Parking.API.Application.DTOS.Responses;
using Parking.API.Application.Validation.Exceptions;
using System.Data;

namespace Parking.API.Application.Entities.Parking.Query
{
    public class ParkingByIdRequest : IRequest<ParkingResponseDTO>
    {
        public Guid Id { get; set; }
    }
    public class ParkingQueryByIdHandler : IRequestHandler<ParkingByIdRequest, ParkingResponseDTO>
    {        
        private readonly IDbConnection _dapperSource;

        public ParkingQueryByIdHandler(IDbConnection dapperSource)
        {  
            _dapperSource = dapperSource ?? throw new ArgumentNullException(nameof(dapperSource));
        }

        public async Task<ParkingResponseDTO> Handle(ParkingByIdRequest request, CancellationToken cancellationToken)
        {

            var sql = $"SELECT p.Id Id, v.Id VehicleID,v.Plate Vehicle, ps.Id ParkingSpacesID, ps.Name ParkingSpaces,EntryDate,ExitDate,Second,TotalValue FROM dbo.Parking p " +
         $"JOIN dbo.Vehicle v With(nolock) ON p.VehicleID = v.id " +
         $"JOIN dbo.ParkingSpaces ps With(nolock) ON p.ParkingSpacesID = ps.Id where p.Id =@Id";

            var entities = await _dapperSource.QueryFirstOrDefaultAsync<ParkingResponseDTO>(sql,new {Id = request.Id});
            if (entities == null)
                throw new NotFoundException("N0 existe : "+request.Id );
            return await Task.FromResult(entities);

        }
    }

}

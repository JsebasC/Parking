using MediatR;
using Parking.API.Application.DTOS.Responses;
using System.ComponentModel.DataAnnotations;

namespace Parking.API.Application.Entities.Vehicle.Query
{
    public record VehicleByIdQuery([Required] Guid Id) : IRequest<VehicleResponseDTO>;   
    public record VehicleQuery() : IRequest<List<VehicleResponseDTO>>;
}

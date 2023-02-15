using Parking.API.Domain.DataAccess;

namespace Parking.API.Application.Repositories
{
    public interface IParkingRepository<T> : IRepository<Parking.API.Domain.Entities.Parking>
    {
        Task<Domain.Entities.Parking> GetAllPayVehicle(string Plate);             
        
    }
}

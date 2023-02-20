using Parking.API.Domain.Entities;
using Parking.API.Domain.Ports;

namespace Parking.API.Domain.Services
{
    public class ParkingService
    {

        readonly IVehicleRepository<Vehicle> _repositoryVehicle;        
        
        public ParkingService(IRepository<Vehicle> repositoryVehicle, IRepository<Entities.Parking> repositoryParking)
        {            
            _repositoryVehicle = _repositoryVehicle ?? throw new ArgumentNullException(nameof(_repositoryVehicle), "No repo available");
        }


        public async Task InsertParkingVehicle(Vehicle vehicleEntity, Guid idParkingSpace)
        {
          await _repositoryVehicle.InsertParkingVehicle(vehicleEntity, idParkingSpace);
        }
    }
}


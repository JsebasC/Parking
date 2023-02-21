using Parking.API.Domain.Entities;
using Parking.API.Domain.Ports;

namespace Parking.API.Domain.Services
{
    public class VehicleParkingService
    {

        readonly IVehicleRepository<Vehicle> _repositoryVehicle;        
        
        public VehicleParkingService(IVehicleRepository<Vehicle> repositoryVehicle)
        {            
            _repositoryVehicle = repositoryVehicle ?? throw new ArgumentNullException(nameof(repositoryVehicle), "No repo available");
        }

        public async Task InsertParkingVehicle(Vehicle vehicleEntity, Guid idParkingSpace)
        {
          await _repositoryVehicle.InsertParkingVehicle(vehicleEntity, idParkingSpace);
        }
    }
}


using Parking.API.Domain.Ports;

namespace Parking.API.Domain.Services
{
    public class ParkingService
    {
        readonly IParkingRepository<Domain.Entities.Parking> _repository;

        public ParkingService(IParkingRepository<Domain.Entities.Parking> repositoryVehicle)
        {
            _repository = repositoryVehicle ?? throw new ArgumentNullException(nameof(repositoryVehicle), "No repo available");
        }
        public async Task PayParkingVehicle(Guid idParking, Entities.Parking parking)
        {
            await _repository.PayParkingVehicle(idParking, parking);
        }
    }
}

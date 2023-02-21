namespace Parking.API.Domain.Ports
{
    public interface IParkingRepository<T> : IRepository<Entities.Parking>
    {
        Task<Entities.Parking> GetAllPayVehicle(string Plate);

        Task PayParkingVehicle(Guid idParking, Entities.Parking parking);
    }
}

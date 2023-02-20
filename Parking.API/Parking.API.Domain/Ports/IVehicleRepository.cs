namespace Parking.API.Domain.Ports
{
    public interface IVehicleRepository<T> : IRepository<Entities.Vehicle>
    {
        Task InsertParkingVehicle(Entities.Vehicle entity, Guid idParkingSpace);
    }
}

using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.Entities;
using Parking.API.Domain.Ports;
using Parking.API.Infrastructure.Data;
using Parking.API.Infrastructure.Repository;

namespace Parking.API.Infrastructure.Adapters
{
    public class VehicleRepository<T> : Repository<Domain.Entities.Vehicle>, IVehicleRepository<T>
    {
        private readonly ParkingContext _context;   
        public VehicleRepository(ParkingContext context) : base(context)
        {
            this._context = context;            
        }

        public async Task InsertParkingVehicle(Vehicle entity, Guid idParkingSpace)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Vehicle!.Add(entity);
                    await _context.SaveChangesAsync();
                    Vehicle vehicle = entity;

                    var Parking = new Domain.Entities.Parking()
                    {
                        ParkingSpacesID = idParkingSpace,
                        VehicleID = vehicle.Id
                    };

                    _context.Parking!.Add(Parking);
                    await _context.SaveChangesAsync();
                    
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {                    
                    transaction.Rollback();
                    throw new LogicException(e.Message);
                }
            }
        }
    }
}

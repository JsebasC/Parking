using Microsoft.EntityFrameworkCore;
using Parking.API.Application.Repositories;
using Parking.API.Domain.DataAccess;
using Parking.API.Infrastructure.Data;
using Parking.API.Infrastructure.Repository;

namespace Parking.API.Application.Services
{
    public class ParkingRepository<T> : Repository<Parking.API.Domain.Entities.Parking>, IParkingRepository<T>
    {
        private readonly ParkingContext _context;
        public ParkingRepository(ParkingContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Domain.Entities.Parking> GetAllPayVehicle(string Plate)
        {
           return await _context.Parking.Include("Vehicle").AsNoTracking().Include("ParkingSpaces").Where(x=> x.Vehicle.Plate == Plate && x.ExitDate == null).FirstOrDefaultAsync();            
        }

    }
}

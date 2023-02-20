using Microsoft.EntityFrameworkCore;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.Entities.Parking.Command;
using Parking.API.Application.Entities.Vehicle.Command;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.Ports;
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
            var result = await _context.Parking!.Include("Vehicle").AsNoTracking().Include("ParkingSpaces").
                Where(x => x.Vehicle!.Plate == Plate && x.ExitDate == null).FirstOrDefaultAsync();

            if (result == null)
                throw new NotFoundException("The entity is null");            
            return result;
        }       
    }
}

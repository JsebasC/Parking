using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.API.Application.Entities.Parking.Command;
using Parking.API.Application.Utils;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.Entities;
using Parking.API.Domain.Ports;
using Parking.API.Infrastructure.Data;
using Parking.API.Infrastructure.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

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
                    Utils.PlateOddEvenDay(entity.Plate, entity.VehicleType, Utils.OddEvenDay());
                   
                    var find = await _context.Vehicle!.AsTracking().FirstOrDefaultAsync(x => x.Plate == entity.Plate);  
                                      
                    if (find != null)
                    {
                        if (await _context.Parking!.FirstOrDefaultAsync(x => x.VehicleID == find.Id && x.ExitDate ==null ) !=null )
                            throw new LogicException("El vehiculo ya existe en el parqueadero");

                        entity = find;
                        _context.Vehicle!.Update(find);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Vehicle!.Add(entity);
                        await _context.SaveChangesAsync();
                    }
                   
                    Vehicle vehicle = entity;                    
                    var Parking = new Domain.Entities.Parking()
                    {
                        ParkingSpacesID = idParkingSpace,
                        VehicleID = vehicle.Id,
                        EntryDate = DateTime.Now
                    };

                    _context.Parking!.Add(Parking);
                    await _context.SaveChangesAsync();

                    var entitySpace = await _context.ParkingSpaces!.FirstOrDefaultAsync(y => y.Id == idParkingSpace);
                    entitySpace!.BusySpace = Utils.QuantitySpace(true, entitySpace);

                    _context.ParkingSpaces!.Update(entitySpace); //Actualizo Espacios
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

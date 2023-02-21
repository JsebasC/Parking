using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.Application.Entities.Parking.Command;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.Ports;
using Parking.API.Infrastructure.Data;
using Parking.API.Infrastructure.Repository;
using System.Data;

namespace Parking.API.Application.Services
{
    public class ParkingRepository<T> : Repository<Parking.API.Domain.Entities.Parking>, IParkingRepository<T>
    {
        private readonly ParkingContext _context;

        private readonly IDbConnection _dapperSource;

        public ParkingRepository(ParkingContext context, IDbConnection dapperSource) : base(context)
        {
            this._context = context;
            this._dapperSource = dapperSource;
        }

        /// <summary>
        /// Obtener todos los vehiculos 
        /// </summary>
        /// <param name="Plate"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Domain.Entities.Parking> GetAllPayVehicle(string Plate)
        {           
            var result = await _context.Parking!.Include("Vehicle").AsNoTracking().Include("ParkingSpaces").
                Where(x => x.Vehicle!.Plate == Plate && x.ExitDate == null).FirstOrDefaultAsync();

            if (result == null)
                throw new NotFoundException("The entity is null");
            return result;
        }

        /// <summary>
        /// Guardar el vehiculo Con el calculo a pagar
        /// </summary>
        /// <param name="idParking"></param>
        /// <param name="parking"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="LogicException"></exception>
        public async Task PayParkingVehicle(Guid idParking, Domain.Entities.Parking parking)
        {
            if (idParking != parking.Id)
                throw new NotFoundException(nameof(idParking), parking!.Id);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var result = await _context.Parking!.FirstOrDefaultAsync(x => x.Id == idParking);
                    if (result == null)
                        throw new NotFoundException(nameof(result), result!.Id);

                    result.ExitDate = parking.ExitDate;

                    LogicValidationParking logicValidationParking = new LogicValidationParking(_dapperSource);
                    Utils.Utils.ExitValidation(result.EntryDate, result.ExitDate);
                    var getDifferenceExit = Utils.Utils.GetDifferenceExit(result.EntryDate, result.ExitDate);
                    
                    result.Second = (long)getDifferenceExit.TotalSeconds;
                    result.TotalValue = logicValidationParking.CalculateValueTotal(result.EntryDate, result.ExitDate, getDifferenceExit, result.VehicleID);

                    _context.Parking!.Update(result); //Actualizo Parking
                    await _context.SaveChangesAsync();

                    var entitySpace = await _context.ParkingSpaces!.FirstOrDefaultAsync(y => y.Id == result.ParkingSpacesID);
                    entitySpace!.BusySpace = Utils.Utils.QuantitySpace(false, entitySpace);

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

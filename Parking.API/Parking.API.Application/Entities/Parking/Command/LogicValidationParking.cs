using Dapper;
using Parking.API.Domain.Entities;
using System.Data;

namespace Parking.API.Application.Entities.Parking.Command
{
    public class LogicValidationParking
    {
        private readonly IDbConnection _dapperSource;
        public LogicValidationParking(IDbConnection dapperSource)
        {
            _dapperSource = dapperSource;
        }
  
        /// <summary>
        /// Manejar el espacio del parqueadero
        /// </summary>
        /// <param name="idParking"></param>
        /// <param name="typeVehicle"></param>
        /// <param name="flagManage"></param>
        /// <exception cref="Validation.Exceptions.NotFoundException"></exception>
        /// <exception cref="Validation.Exceptions.LogicException"></exception>
        public void ManageSpace(Guid idParking, int typeVehicle, bool flagManage)
        {
            var sql = "select * from dbo.ParkingSpaces where Id = @Id and VehicleType =@VehicleType ";
            var entitySpace = _dapperSource.QuerySingleOrDefault<Domain.Entities.ParkingSpaces>(sql, new { Id = idParking, VehicleType = typeVehicle });        
            if (entitySpace == null)
                throw new Validation.Exceptions.NotFoundException($"El espacio no corresponde al tipo de vehiculo");
            QuantitySpace(flagManage, entitySpace);
        }

        /// <summary>
        /// Manejar la cantidad de espacios por bloques
        /// </summary>
        /// <param name="idParking"></param>
        /// <param name="flagManage"></param>
        /// <exception cref="Validation.Exceptions.NotFoundException"></exception>
        public void ManageSpace(Guid idParking, bool flagManage)
        {
            var sql = "select * from dbo.ParkingSpaces where Id = @Id ";
            var entitySpace = _dapperSource.QuerySingleOrDefault<Domain.Entities.ParkingSpaces>(sql, new { Id = idParking });
            if (entitySpace == null)
                throw new Validation.Exceptions.NotFoundException($"El espacio no corresponde al tipo de vehiculo");
            QuantitySpace(flagManage, entitySpace);
        }

        /// <summary>
        /// Si sale o entra
        /// </summary>
        /// <param name="flagManage"></param>
        /// <param name="entitySpace"></param>
        /// <exception cref="Validation.Exceptions.LogicException"></exception>
        private void QuantitySpace(bool flagManage,Domain.Entities.ParkingSpaces entitySpace)
        {
            int? spaceBusy = 0;
            if (flagManage)
            {
                spaceBusy = ++entitySpace.BusySpace;
                if (spaceBusy > entitySpace.BusySpace)
                    throw new Validation.Exceptions.LogicException($"El {entitySpace.Name} llego a su limite");
            }
            else
            {
                spaceBusy = --entitySpace.BusySpace;
                if (spaceBusy < entitySpace.BusySpace)
                    throw new Validation.Exceptions.LogicException($"No puedes darle mas salida al bloque {entitySpace.Name}");
            }

            _dapperSource.Execute(
               "UPDATE dbo.ParkingSpaces SET BusySpace = @BusySpace WHERE id = @id",
               new { BusySpace = spaceBusy, Id = entitySpace.Id });
        }


        #region MyRegion


        /// <summary>
        /// Validacion d salida
        /// </summary>
        /// <param name="entryDate"></param>
        /// <param name="exitDate"></param>
        /// <param name="entity"></param>
        /// <exception cref="Validation.Exceptions.LogicException"></exception>
        public decimal? CalculateValueTotal(DateTime entryDate, DateTime? exitDate, TimeSpan getDifferenceExit, Guid VehicleID)
        {
            var vehicle = _dapperSource.QuerySingleOrDefault<Domain.Entities.Vehicle>("select * from dbo.Vehicle where Id = @Id", new { Id = VehicleID });
            var rateParking = GetRate(vehicle.VehicleType);
            if (vehicle.VehicleType == 0) //0 : moto                
                return CalculateValueMot(getDifferenceExit, vehicle.CubicCentimeters, rateParking);
            if (vehicle.VehicleType == 1)//1 : carros
                return CalculateValueCar(getDifferenceExit, rateParking);

            return 0;
        }


        private Tuple<decimal, decimal> GetRate(int vehicleType)
        {
            var rateParking = _dapperSource.QueryAsync<Domain.Entities.ParkingRate>("select * from dbo.ParkingRate where VehicleType =  @VehicleType", new { VehicleType = vehicleType });
            if (rateParking.Result.Count() == 0)
                throw new Validation.Exceptions.LogicException("No hay tarifas parametrizadas");
            //Nombre,Valor,Horario(0: hora, 1:Dia), Tipo vehiculo(0:Moto,1:carro)
            decimal rateHours = rateParking.Result.Where(x => x.Time == 0).Select(y => y.Value).FirstOrDefault();
            decimal rateday = rateParking.Result.Where(x => x.Time == 1).Select(y => y.Value).FirstOrDefault();
            return Tuple.Create(rateHours, rateday);
        }

        /// <summary>
        /// Calcular el valor de la moto a pagar
        /// </summary>
        /// <param name="getDifferenceExit"></param>
        /// <param name="CubicCentimeters"></param>
        /// <param name="rateParking"></param>
        /// <returns></returns>
        private decimal? CalculateValueMot(TimeSpan getDifferenceExit, int? CubicCentimeters, Tuple<decimal, decimal> rateParking)
        {
            decimal? TotalValue = CalculateHoursAndDay(getDifferenceExit, rateParking.Item1, rateParking.Item2);
            if (CubicCentimeters > 500) //Sobrecargo
                TotalValue += 2000;

            return TotalValue;
        }

        /// <summary>
        /// Calcular el valor del carro
        /// </summary>
        /// <param name="getDifferenceExit"></param>
        /// <param name="rateParking"></param>
        /// <returns></returns>
        private decimal? CalculateValueCar(TimeSpan getDifferenceExit, Tuple<decimal, decimal> rateParking)
        {
            decimal? TotalValue = CalculateHoursAndDay(getDifferenceExit, rateParking.Item1, rateParking.Item2);
            return TotalValue;
        }

        /// <summary>
        /// Cobrar 
        /// </summary>
        /// <param name="getDifferenceExit"></param>
        /// <param name="rateHours"></param>
        /// <param name="rateDay"></param>
        /// <returns></returns>
        private decimal? CalculateHoursAndDay(TimeSpan getDifferenceExit, decimal rateHours, decimal rateDay)
        {
            decimal? TotalValue = 0;
            int minutes = (int)getDifferenceExit.Minutes;
            int horas = (int)getDifferenceExit.Hours;
            int dias = (int)getDifferenceExit.Days;

            if (minutes > 0 && minutes <= 60) //cobro fracciones en horas
                TotalValue += rateHours;

            if (horas > 0 && horas < 9) //se cobra por horas si el tiempo es menos a 9
                TotalValue += horas * rateHours;

            if (horas >= 9 && horas <= 24) //se aumenta un dia cuando las horas sean de 9 a 24 horas de parqueo
                dias++;

            if (dias > 0) //
                TotalValue += dias * rateDay;

            return TotalValue;
        }

        #endregion

    }




}



//var sql = @"select p.*, V.* from dbo.Parking p JOIN dbo.Vehicle v With(nolock) ON p.VehicleID = v.id where P.Id = @Ids";
//var parameters = new DynamicParameters();
//parameters.Add("@Ids", request.Id);

//var parkingResult = await _dapperSource.QueryAsync<Domain.Entities.Parking, Domain.Entities.Vehicle, Domain.Entities.Parking>(
//      sql,
//      (parking, vehicle) => {
//          parking.Vehicle = vehicle;
//          return parking;
//      }, splitOn: "VehicleID");

//var parking = parkingResult.FirstOrDefault();
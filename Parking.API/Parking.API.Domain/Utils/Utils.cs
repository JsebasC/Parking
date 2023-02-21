using System.Text.RegularExpressions;

namespace Parking.API.Application.Utils
{
    public class Utils
    {
        /// <summary>
        /// Validar si el vehiculo entra
        /// </summary>
        /// <param name="plate"></param>
        /// <param name="vehicleType"></param>
        /// <param name="FlagTodayOddEvenDay"></param>
        /// <exception cref="Validation.Exceptions.LogicException"></exception>
        public static void PlateOddEvenDay(string plate, int vehicleType, bool FlagTodayOddEvenDay)
        {
            int numero = GetNumberPlate(plate, vehicleType);
            string mensaje = string.Empty;
            string dateString = DateTime.Now.ToString("D");
            if ((numero % 2 == 0) && !FlagTodayOddEvenDay)
            {
                mensaje += "El vehiculo no puede entrar hoy,";
                mensaje += string.Format("el numero es par y hoy entran los impares segun el dia de hoy : {0} ", dateString);
                throw new Validation.Exceptions.LogicException(mensaje);
            }

            if (!(numero % 2 == 0) && FlagTodayOddEvenDay)
            {
                mensaje += "El vehiculo no puede entrar hoy,";
                mensaje += string.Format("el numero es impar y hoy entran los pares segun el dia de hoy : {0}", dateString);
                throw new Validation.Exceptions.LogicException(mensaje);
            }
        }

        /// <summary>
        /// Obtener minutos transcurridos en el parqueadero
        /// </summary>
        /// <param name="entryDate"></param>
        /// <param name="exitDate"></param>
        /// <returns></returns>
        public static TimeSpan GetDifferenceExit(DateTime entryDate, DateTime? exitDate)
        {
            TimeSpan difference = (TimeSpan)(exitDate-entryDate)!;
            return difference;
        }

        /// <summary>
        /// Obtener si el dia es par o impar
        /// </summary>
        /// <returns></returns>
        public static bool OddEvenDay()
        {
            if (DateTime.Today.Day % 2 == 0)
                return true; //dia par                
            return false;
        }

        /// <summary>
        /// Obtener el numero de la placa segun el vehiculo
        /// </summary>
        /// <param name="plate"></param>
        /// <param name="vehicleType"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int GetNumberPlate(string plate, int vehicleType)
        {
            try
            {
                int numero = 0;
                if (vehicleType == 0) //moto
                {
                    string pattern = @"\d+";
                    Match match = Regex.Match(plate, pattern);
                    if (match.Success)
                    {
                        numero = Convert.ToInt32(match.Value.First().ToString());
                    }
                }
                else//carro
                {
                    numero = Convert.ToInt32(plate.Substring(plate.Length - 1, 1));
                }
                return numero;
            }
            catch (Exception)
            {

                throw new Validation.Exceptions.LogicException($"La combinación de placas no corresponde a un vehiculo valido");
            }
        }

        /// <summary>
        /// Validar la salida
        /// </summary>
        /// <param name="entryDate"></param>
        /// <param name="exitDate"></param>
        /// <param name="entity"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void ExitValidation(DateTime entryDate, DateTime? exitDate)
        {
            if (entryDate > exitDate)
                throw new Validation.Exceptions.LogicException($"La fecha de entrada no puede ser mayor a la de salida");
        }

        /// <summary>
        /// Si sale o entra
        /// </summary>
        /// <param name="flagManage"></param>
        /// <param name="entitySpace"></param>
        /// <exception cref="Validation.Exceptions.LogicException"></exception>
        public static int? QuantitySpace(bool flagManage, Domain.Entities.ParkingSpaces entitySpace)
        {
            int? spaceBusy = entitySpace.BusySpace;
            if (flagManage)
            {
                spaceBusy = ++spaceBusy;
                if (spaceBusy > entitySpace.Space)
                    throw new Validation.Exceptions.LogicException($"El {entitySpace.Name} llego a su limite");
            }
            else
            {
                spaceBusy = --spaceBusy;
                if (spaceBusy < 0)
                    throw new Validation.Exceptions.LogicException($"No puedes darle mas salida al bloque {entitySpace.Name}");
            }
            return spaceBusy;
        }


    }


}

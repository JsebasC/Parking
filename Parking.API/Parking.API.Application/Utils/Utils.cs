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
    }

    
}

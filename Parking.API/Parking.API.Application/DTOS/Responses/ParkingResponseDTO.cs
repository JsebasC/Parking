using Parking.API.Domain.Entities.Base;

namespace Parking.API.Application.DTOS.Responses
{
    public class ParkingResponseDTO : EntityBase<Guid>
    {
        public Guid VehicleID { get; set; }
        public string? Vehicle { get; set; }
        public Guid ParkingSpacesID { get; set; }
        public string? ParkingSpaces { get; set; }
        public DateTime EntryDate { get; set; }       
        public DateTime? ExitDate { get; set; }
        [Second]
        public long? Second { get; set; }
        public string? Duration
        {
            get
            {
                var vehicleTypeAttribute = (Second?)Attribute.GetCustomAttribute(typeof(ParkingResponseDTO).GetProperty("Second")!, typeof(Second));
                return vehicleTypeAttribute!.GetGenderString(ExitDate, EntryDate);
            }
        }
        public decimal? TotalValue { get; set; }
        [VehicleType]
        public int VehicleType { get; set; }
        public string VehicleTypeName
        {
            get
            {
                var vehicleTypeAttribute = (VehicleType?)Attribute.GetCustomAttribute(typeof(ParkingResponseDTO).GetProperty("VehicleType")!, typeof(VehicleType));
                return vehicleTypeAttribute!.GetGenderString(VehicleType);
            }
        }
        public int CubicCentimeters { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Second : Attribute
    {
        public string GetGenderString(DateTime? exitDate, DateTime entryDate)
        {
            if (exitDate == null)
                return "No ha salido";

            TimeSpan difference = (TimeSpan)(exitDate - entryDate);
            string message = string.Format("{0} Dias con {1} Horas con {2} Minutos", difference.Days, difference.Hours, difference.Minutes);
            return message;
        }
    }
}

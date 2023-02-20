namespace Parking.API.Application.DTOS.Request
{
    public class ParkingVehicleDTO
    {
        public string Plate { get; set; } = string.Empty;
        public int? CubicCentimeters { get; set; }
        public int VehicleType { get; set; }
        public string? idParkingSpace { get; set; }

    }

}

using Parking.API.Domain.Entities.Base;

namespace Parking.API.Application.DTOS.Request
{
    public class ParkingSpaceDTO 
    {
        public string? Name { get; set; }
        public int Space { get; set; }
        //public int? BusySpace { get; set; }
        public int VehicleType { get; set; }
    }
}

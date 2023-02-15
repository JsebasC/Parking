using Parking.API.Domain.Entities.Base;

namespace Parking.API.Domain.Entities
{
    public class ParkingRate : EntityBase<Guid>
    {        
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int Time { get; set; } //Hora        
        public int VehicleType { get; set; }

    }
}

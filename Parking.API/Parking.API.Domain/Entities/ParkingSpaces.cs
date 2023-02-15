using Parking.API.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking.API.Domain.Entities
{
    [Table("ParkingSpaces", Schema = "dbo")]
    public class ParkingSpaces : EntityBase<Guid>
    {        
        public string? Name { get; set; }
        public int Space { get; set; }
        public int? BusySpace { get; set; }
        public int VehicleType { get; set; }        
    }
}

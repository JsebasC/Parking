using Parking.API.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking.API.Domain.Entities
{
    [Table("Parking", Schema = "dbo")]
    public class Parking : EntityBase<Guid>
    {        
        [ForeignKey("Vehicle")]
        public Guid VehicleID { get; set; }
        [ForeignKey("ParkingSpaces")]
        public Guid ParkingSpacesID { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public long? Second { get; set; }
        public decimal? TotalValue { get; set; }
        //Relations
        public Vehicle? Vehicle { get; set; }

        public ParkingSpaces? ParkingSpaces { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Parking.API.Domain.Entities.Base;

namespace Parking.API.Domain.Entities
{

    [Table("Vehicle", Schema="dbo")]
    public class Vehicle : EntityBase<Guid>
    {
        [Required] 
        [StringLength(6)]
        public string Plate { get; set; } = string.Empty;        
        public int? CubicCentimeters { get; set; }
        public int VehicleType { get; set; }
   
    }
}

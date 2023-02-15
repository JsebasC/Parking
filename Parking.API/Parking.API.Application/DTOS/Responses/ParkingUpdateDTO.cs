using Parking.API.Domain.Entities.Base;

namespace Parking.API.Application.DTOS.Responses
{
    public class ParkingUpdateDTO : EntityBase<Guid>
    {
        //public Guid VehicleID { get; set; }
        //public Guid ParkingSpacesID { get; set; }
        //public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        //public int? Minutes { get; set; }
        //public decimal? TotalValue { get; set; }

    }
}

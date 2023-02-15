using Parking.API.Application.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Parking.API.Application.DTOS.Request
{
    public class ParkingDTO 
    {        
        public Guid VehicleID { get; set; }
        public Guid ParkingSpacesID { get; set; }
        //public DateTime EntryDate { get; set; }

    }
}

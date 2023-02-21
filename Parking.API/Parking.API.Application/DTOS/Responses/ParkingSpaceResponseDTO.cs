using Parking.API.Application.DTOS.Responses;
using Parking.API.Domain.Entities.Base;
using VehicleType = Parking.API.Application.DTOS.Responses.VehicleType;

namespace Parking.API.Application.DTOS.Request
{
    public class ParkingSpaceResponseDTO : EntityBase<Guid>
    {

        public string? Name { get; set; }
        public int Space { get; set; }
        public int? BusySpace { get; set; }

        [VehicleType]
        public int VehicleType { get; set; }
        public string VehicleTypeName
        {
            get
            {
                var vehicleTypeAttribute = (VehicleType?)Attribute.GetCustomAttribute(typeof(VehicleResponseDTO).GetProperty("VehicleType")!, typeof(VehicleType));
                return vehicleTypeAttribute!.GetVehicleString(VehicleType);
            }
        }

    }
}

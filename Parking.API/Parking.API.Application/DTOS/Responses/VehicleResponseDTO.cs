using Parking.API.Domain.Entities.Base;

namespace Parking.API.Application.DTOS.Responses
{    
    public class VehicleResponseDTO : EntityBase<Guid>
    {
        public string Plate { get; set; } = string.Empty;
        public int? CubicCentimeters { get; set; }

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

    [AttributeUsage(AttributeTargets.Property)]
    public class VehicleType : Attribute
    {
        public string GetVehicleString(int VehicleType)
        {
            switch (VehicleType)
            {
                case 0:
                    return "Moto";
                case 1:
                    return "Carro";
                default:
                    return "Unknown";
            }
        }
    }

}

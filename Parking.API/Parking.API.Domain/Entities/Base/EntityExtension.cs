namespace Parking.API.Domain.Entities.Base
{
    public enum VehicleType
    {
        M, C //Moto y Carro
    }
    public class EntityExtension<T> 
    {        
        public VehicleType VehicleType { get; set; }

    }
}

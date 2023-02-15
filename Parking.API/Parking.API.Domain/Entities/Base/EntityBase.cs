namespace Parking.API.Domain.Entities.Base
{
    public class EntityBase<T> 
    {
        public virtual T Id { get; set; } = default!;
    }
}

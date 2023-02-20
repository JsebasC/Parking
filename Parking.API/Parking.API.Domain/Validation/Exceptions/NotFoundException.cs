namespace Parking.API.Application.Validation.Exceptions
{
    //Esta excepción nos servirá para cuando se busque un Entity y este no exista.    
    public class NotFoundException : Exception
    {
        public NotFoundException()
        : base()
        {
        }
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }


}

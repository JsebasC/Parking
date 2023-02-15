using FluentValidation.Results;


namespace Parking.API.Application.Validation.Exceptions
{
    // Exceptions personalizadas
    public class ValidationException : Exception
    {
        //Esta Exception sirve para guardar los errores de validación que pueden ocurrir al querer procesesar una solicitud.
        public ValidationException()
        : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(string? message) : base(message)
        {
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}

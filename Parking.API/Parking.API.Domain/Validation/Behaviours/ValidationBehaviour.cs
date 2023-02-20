using FluentValidation;
using MediatR;
using ValidationException = Parking.API.Application.Validation.Exceptions.ValidationException;

namespace Parking.API.Application.Validation.Behaviours
{
    //Los Behaviors de MediatR tienen el mismo funcionamiento que hace un Middleware Ejecuta algo y delega la ejecución al siguiente (y queda en espera de la respuesta).
    //al ejecutar un IRequest<T>, podemos decir que haga x o y antes de ejecutar el Handler determinado por el mediador.
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        //Lo que hace este decorador es recibir el Request y validarlo con todos los posibles validadores de FluentValidation que se encuentran en el proyecto.
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any())
                    throw new ValidationException(failures);
            }
            return await next();
        }
    }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Parking.API.Application.Entities.Vehicle.Command;
using Parking.API.Application.Services;
using Parking.API.Application.Validation.Behaviours;
using Parking.API.Domain.Ports;
using Parking.API.Infrastructure.Repository;
using System.Reflection;

namespace Parking.API.Infrastructure.HandlerDependency
{
    public static class DependencyInjection
    {

        /// <summary>
        /// Agregar los request
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        public static void AddRequestDependency(this IServiceCollection services, Assembly assembly)
        {

            var classTypes = AppDomain.CurrentDomain.GetAssemblies()
               .Where(assembly =>
               {
                   return (assembly.FullName is null) ? false : assembly.FullName.Contains("Parking.API.Domain.DataAccess", StringComparison.InvariantCulture);
               })
               .SelectMany(s => s.GetTypes())
               .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(IRequest)));

            foreach (var type in classTypes)
            {
                services.AddTransient(type);
            }
        }
        /// <summary>
        /// Agregar los handler
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        public static void AddMediatRHandlers(this IServiceCollection services, Assembly assembly)
        {
            var classTypes = AppDomain.CurrentDomain.GetAssemblies()
               .Where(assembly =>
               {
                   return (assembly.FullName is null) ? false : assembly.FullName.Contains("Parking.API.Application.Entities", StringComparison.InvariantCulture);
               })
               .SelectMany(s => s.GetTypes())
               .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(IRequestHandler<,>)));

            foreach (var type in classTypes)
            {
                services.AddTransient(type);
            }

        }

        /// <summary>
        /// Agregar el repositorio
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositoryDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IParkingRepository<>), typeof(ParkingRepository<>));

        }

        /// <summary>
        /// Agregar validadores de los hanlder
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>)); // Update
            var assembly = typeof(VehicleCreateValidator).Assembly;            
            var validatorTypes = assembly.GetExportedTypes()
                    .Where(t => t.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)));

            foreach (var validatorType in validatorTypes)
            {
                var requestType = validatorType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
                    ?.GetGenericArguments()
                    .FirstOrDefault();

                if (requestType == null) continue;

                var registration = services.FirstOrDefault(sd => sd.ServiceType == typeof(IValidator<>) && sd.ImplementationType == validatorType);
                if (registration != null) continue;

                services.AddTransient(typeof(IValidator<>).MakeGenericType(requestType), validatorType);
            }

            return services;
        }
    }        
}

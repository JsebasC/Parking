using MediatR;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.DataAccess;

namespace Parking.API.Application.Entities.Vehicle.Command
{
    public class VehicleDeleteRequest : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class ParkingRateDeleteHandler : IRequestHandler<VehicleDeleteRequest, bool>
    {
        private readonly IRepository<Domain.Entities.Vehicle> _repository;

        public ParkingRateDeleteHandler(IRepository<Domain.Entities.Vehicle> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(VehicleDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id);
            if (entity == null)
                throw new NotFoundException("The entity is null");

            await _repository.Delete(request.Id);
            return true;
        }
    }
}

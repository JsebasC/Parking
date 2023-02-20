using MediatR;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.Ports;

namespace Parking.API.Application.Entities.ParkingSpaces.Command
{
    public class ParkingSpaceDeleteRequest : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class ParkingRateDeleteHandler : IRequestHandler<ParkingSpaceDeleteRequest, bool>
    {
        private readonly IRepository<Domain.Entities.ParkingSpaces> _repository;

        public ParkingRateDeleteHandler(IRepository<Domain.Entities.ParkingSpaces> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(ParkingSpaceDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id);
            if (entity == null)
                throw new NotFoundException("The entity is null");

            await _repository.Delete(request.Id);
            return true;
        }
    }
}

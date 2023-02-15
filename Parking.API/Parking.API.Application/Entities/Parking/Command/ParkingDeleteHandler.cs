using MediatR;
using Parking.API.Application.Validation.Exceptions;
using Parking.API.Domain.DataAccess;

namespace Parking.API.Application.Entities.Parking.Command
{
    public class ParkingDeleteRequest : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class ParkingDeleteHandler : IRequestHandler<ParkingDeleteRequest, bool>
    {
        private readonly IRepository<Domain.Entities.Parking> _repository;

        public ParkingDeleteHandler(IRepository<Domain.Entities.Parking> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(ParkingDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id);
            if (entity == null)
                throw new NotFoundException("The entity is null");

            await _repository.Delete(request.Id);
            return true;
        }
    }
}

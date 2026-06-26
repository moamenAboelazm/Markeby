using Library.IRepository;
using MediatR;

namespace Library.Features.Trips.Queries
{
    public class CheckAvailableSeatsQuery : IRequest<bool>
    {
        public Guid TripId { get; set; }
        public int RequiredSeats { get; set; }
    }

    public class CheckAvailableSeatsQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<CheckAvailableSeatsQuery, bool>
    {
        public async Task<bool> Handle(CheckAvailableSeatsQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Trips.HasAvailableSeatsAsync(data.TripId, data.RequiredSeats);
        }
    }
}

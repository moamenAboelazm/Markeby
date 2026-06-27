using Library.IRepository;
using MediatR;

namespace Library.Features.Bookings.Queries
{
    public class CheckUserBookedTripQuery : IRequest<bool>
    {
        public string UserId { get; set; }
        public Guid TripId { get; set; }
    }

    public class CheckUserBookedTripQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<CheckUserBookedTripQuery, bool>
    {
        public async Task<bool> Handle(CheckUserBookedTripQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Bookings.HasUserBookedTripAsync(data.UserId, data.TripId);
        }
    }
}

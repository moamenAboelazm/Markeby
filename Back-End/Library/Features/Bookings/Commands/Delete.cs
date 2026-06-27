using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Bookings.Commands
{
    public class DeleteBookingCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteBookingCommandHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) : IRequestHandler<DeleteBookingCommand, bool>
    {
        public async Task<bool> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(request.Id);
            if (booking == null) return false;

            if (string.IsNullOrEmpty(booking.CancellationReason))
            {
                var trip = await _unitOfWork.Trips.GetByIdAsync(booking.TripId);
                if (trip != null)
                {
                    trip.AvailableSeats += booking.NumberOfTickets;
                    _unitOfWork.Trips.Update(trip);
                }
            }

            _unitOfWork.Bookings.Delete(booking);
            await _unitOfWork.CompleteAsync();

            _cache.Remove($"UserBookingsCacheKey_{booking.UserId}");
            _cache.Remove("AvailableUpcomingTripsCacheKey");
            _cache.Remove($"TripDetailsCacheKey_{booking.TripId}");
            _cache.Remove("AllBookingsCacheKey");
            _cache.Remove("AllTripsCacheKey");

            return true;
        }
    }
}
using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Bookings.Commands
{
    public class UpdateBookingCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string? SpecialRequests { get; set; }
        public string? CancellationReason { get; set; }
    }

    public class UpdateBookingCommandHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) : IRequestHandler<UpdateBookingCommand, bool>
    {
        public async Task<bool> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(request.Id);
            if (booking == null) return false;

            if (!string.IsNullOrEmpty(booking.CancellationReason))
                throw new InvalidOperationException("Cannot update an already cancelled booking.");

            booking.SpecialRequests = request.SpecialRequests;

            if (!string.IsNullOrEmpty(request.CancellationReason))
            {
                booking.CancellationReason = request.CancellationReason;

                var trip = await _unitOfWork.Trips.GetByIdAsync(booking.TripId);
                if (trip != null)
                {
                    trip.AvailableSeats += booking.NumberOfTickets;
                    _unitOfWork.Trips.Update(trip);
                }
            }

            _unitOfWork.Bookings.Update(booking);
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
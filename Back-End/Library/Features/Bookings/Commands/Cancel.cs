using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

public class CancelBookingCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string CancellationReason { get; set; } = "Cancelled by Customer";
}

public class CancelBookingCommandHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) : IRequestHandler<CancelBookingCommand, bool>
{
    public async Task<bool> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(request.Id);
        if (booking == null || !string.IsNullOrEmpty(booking.CancellationReason))
            return false; 

        booking.CancellationReason = request.CancellationReason;

        var trip = await _unitOfWork.Trips.GetByIdAsync(booking.TripId);
        if (trip != null)
        {
            trip.AvailableSeats += booking.NumberOfTickets;
            _unitOfWork.Trips.Update(trip);
        }

        _unitOfWork.Bookings.Update(booking);
        await _unitOfWork.CompleteAsync();

        _cache.Remove($"UserBookingsCacheKey_{booking.UserId}");
        _cache.Remove("AllBookingsCacheKey");
        _cache.Remove("AllTripsCacheKey");

        return true;
    }
}
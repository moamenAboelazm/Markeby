using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<Guid>
    {
        public string UserId { get; set; } = string.Empty;
        public Guid TripId { get; set; }
        public int NumberOfTickets { get; set; }
        public string? SpecialRequests { get; set; }
    }

    public class CreateBookingCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IMemoryCache _cache) : IRequestHandler<CreateBookingCommand, Guid>
    {
        public async Task<Guid> Handle(CreateBookingCommand data, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.Trips.GetByIdAsync(data.TripId);
            if (trip == null) throw new InvalidOperationException("Trip not found.");

            if (trip.StartTime <= DateTime.UtcNow.AddHours(3))
                throw new InvalidOperationException("Cannot book a past or currently running trip.");

            if (trip.Status != TripStatus.Scheduled)
                throw new InvalidOperationException("This trip is currently not available for booking.");

            if (trip.AvailableSeats < data.NumberOfTickets)
                throw new InvalidOperationException($"Not enough available seats, there are only {trip.AvailableSeats} seats");

            var booking = _mapper.Map<Booking>(data);
            booking.TotalPrice = trip.Price * data.NumberOfTickets;
            booking.BookingDate = DateTime.UtcNow.AddHours(3);

            trip.AvailableSeats -= data.NumberOfTickets;
            _unitOfWork.Trips.Update(trip);

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.CompleteAsync();

            _cache.Remove($"UserBookingsCacheKey_{data.UserId}");
            _cache.Remove("AvailableUpcomingTripsCacheKey");
            _cache.Remove($"TripDetailsCacheKey_{data.TripId}");
            _cache.Remove("AllBookingsCacheKey");
            _cache.Remove("AllTripsCacheKey");

            return booking.Id;
        }
    }
}
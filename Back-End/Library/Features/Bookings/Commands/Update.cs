using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Bookings.Commands
{
    public class UpdateBookingCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string? SpecialRequests { get; set; }
        public string? CancellationReason { get; set; }
    }

    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public UpdateBookingCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<bool> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(request.Id);
            if (booking == null) return false;

            booking.SpecialRequests = request.SpecialRequests;

            if (string.IsNullOrEmpty(booking.CancellationReason) && !string.IsNullOrEmpty(request.CancellationReason))
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

            return true;
        }
    }
}

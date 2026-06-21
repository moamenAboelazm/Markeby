using AutoMapper;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<Guid>
    {
        public string UserId { get; set; }
        public Guid TripId { get; set; }
        public int NumberOfTickets { get; set; }
        public string? SpecialRequests { get; set; }
    }

    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CreateBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.Trips.GetByIdAsync(request.TripId);
            if (trip == null) throw new InvalidOperationException("Trip not found.");

            if (trip.AvailableSeats < request.NumberOfTickets)
                throw new InvalidOperationException("Not enough available seats.");

            var booking = _mapper.Map<Booking>(request);
            booking.TotalPrice = trip.Price * request.NumberOfTickets;
            booking.BookingDate = DateTime.UtcNow;

            trip.AvailableSeats -= request.NumberOfTickets;
            _unitOfWork.Trips.Update(trip);

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.CompleteAsync();

            _cache.Remove($"UserBookingsCacheKey_{request.UserId}");
            _cache.Remove("AvailableUpcomingTripsCacheKey");
            _cache.Remove($"TripDetailsCacheKey_{request.TripId}");

            return booking.Id;
        }
    }
}

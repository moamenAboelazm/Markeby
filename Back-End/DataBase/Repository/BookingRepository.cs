using DataBase.Contexts;
using Library.Enums;
using Library.Features.Bookings;
using Library.IRepository;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DataBase.Repository
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public BookingRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IReadOnlyList<Booking>> GetAllBookingsWithDetailsAsync()
        {
            string cacheKey = "AllBookingsCacheKey";

            if (!_cache.TryGetValue(cacheKey, out IReadOnlyList<Booking>? bookings))
            {
                bookings = await _context.Set<Booking>().Include(b => b.User).Include(b => b.Trip).ThenInclude(t => t.Boat)
                    .AsNoTracking().ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, bookings, cacheOptions);
            }

            return bookings ?? new List<Booking>();
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId)
        {
            return await _context.Set<Booking>().Include(b => b.Trip).ThenInclude(t => t.Boat)
                .Include(b => b.User).FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<SystemDashboardDto> GetSystemDashboardAsync()
        {
            return new SystemDashboardDto
            {
                TotalCompletedTrips = await _context.Set<Trip>().CountAsync(t => t.Status == TripStatus.Completed),
                TotalCancelledTrips = await _context.Set<Trip>().CountAsync(t => t.Status == TripStatus.Cancelled),
                TotalScheduledTrips = await _context.Set<Trip>().CountAsync(t => t.Status == TripStatus.Scheduled),

                TotalSystemProfit = await _context.Set<Booking>().Where(b => string.IsNullOrEmpty(b.CancellationReason)).SumAsync(b => b.TotalPrice)
            };
        }

        public async Task<TripDashboardDto?> GetTripDashboardAsync(Guid tripId)
        {
            var trip = await _context.Set<Trip>().Include(t => t.Boat).AsNoTracking().FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                return null;

            var totalProfit = await _context.Set<Booking>().Where(b => b.TripId == tripId && string.IsNullOrEmpty(b.CancellationReason))
                .SumAsync(b => b.TotalPrice);

            return new TripDashboardDto
            {
                TicketsSold = trip.Boat.Capacity - trip.AvailableSeats,
                TicketsRemaining = trip.AvailableSeats,
                TotalTripProfit = totalProfit
            };
        }

        public async Task<bool> HasUserBookedTripAsync(string userId, Guid tripId)
        {
            return await _context.Set<Booking>().AnyAsync(b => b.UserId == userId && b.TripId == tripId);
        }
    }
}
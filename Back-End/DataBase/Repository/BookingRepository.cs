using DataBase.Contexts;
using Library.IRepository;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IReadOnlyList<Booking>> GetUserBookingsAsync(string userId)
        {
            string cacheKey = $"UserBookingsCacheKey_{userId}";

            if (!_cache.TryGetValue(cacheKey, out IReadOnlyList<Booking>? bookings))
            {
                bookings = await _context.Set<Booking>()
                    .Include(b => b.Trip)
                    .ThenInclude(t => t.Boat)
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.BookingDate)
                    .AsNoTracking()
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, bookings, cacheOptions);
            }

            return bookings ?? new List<Booking>();
        }

        public async Task<IReadOnlyList<Booking>> GetBookingsByTripIdAsync(Guid tripId)
        {
            return await _context.Set<Booking>()
                .Include(b => b.User)
                .Where(b => b.TripId == tripId)
                .OrderByDescending(b => b.BookingDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId)
        {
            return await _context.Set<Booking>()
                .Include(b => b.Trip)
                .ThenInclude(t => t.Boat)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<IReadOnlyList<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Set<Booking>()
                .Include(b => b.Trip)
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .OrderBy(b => b.BookingDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueByTripIdAsync(Guid tripId)
        {
            return await _context.Set<Booking>()
                .Where(b => b.TripId == tripId)
                .SumAsync(b => b.TotalPrice);
        }

        public async Task<bool> HasUserBookedTripAsync(string userId, Guid tripId)
        {
            return await _context.Set<Booking>()
                .AnyAsync(b => b.UserId == userId && b.TripId == tripId);
        }
    }
}

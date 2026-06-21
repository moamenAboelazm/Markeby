using DataBase.Contexts;
using Library.Enums;
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
    public class TripRepository : GenericRepository<Trip>, ITripRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public TripRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IReadOnlyList<Trip>> GetAvailableUpcomingTripsAsync()
        {
            string cacheKey = "AvailableUpcomingTripsCacheKey";

            if (!_cache.TryGetValue(cacheKey, out IReadOnlyList<Trip>? trips))
            {
                trips = await _context.Set<Trip>()
                    .Include(t => t.Boat)
                    .Include(t => t.Captain)
                    .Where(t => t.StartTime > DateTime.UtcNow
                             && t.Status == TripStatus.Scheduled
                             && t.AvailableSeats > 0)
                    .OrderBy(t => t.StartTime)
                    .AsNoTracking()
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(2));

                _cache.Set(cacheKey, trips, cacheOptions);
            }

            return trips ?? new List<Trip>();
        }

        public async Task<Trip?> GetTripWithDetailsByIdAsync(Guid id)
        {
            string cacheKey = $"TripDetailsCacheKey_{id}";

            if (!_cache.TryGetValue(cacheKey, out Trip? trip))
            {
                trip = await _context.Set<Trip>()
                    .Include(t => t.Boat)
                    .Include(t => t.Captain)
                    .Include(t => t.Bookings)
                    .Include(t => t.Passengers)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (trip != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                    _cache.Set(cacheKey, trip, cacheOptions);
                }
            }

            return trip;
        }

        public async Task<IReadOnlyList<Trip>> GetTripsByBoatIdAsync(Guid boatId)
        {
            return await _context.Set<Trip>()
                .Where(t => t.BoatId == boatId)
                .OrderByDescending(t => t.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Trip>> GetTripsByCaptainIdAsync(Guid captainId)
        {
            return await _context.Set<Trip>()
                .Where(t => t.CaptainId == captainId)
                .OrderByDescending(t => t.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Trip>> GetTripsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Set<Trip>()
                .Include(t => t.Boat)
                .Where(t => t.StartTime >= startDate && t.EndTime <= endDate)
                .OrderBy(t => t.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Trip>> SearchTripsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Trip>();

            searchTerm = searchTerm.ToLower();

            return await _context.Set<Trip>()
                .Include(t => t.Boat)
                .Where(t => t.Title.ToLower().Contains(searchTerm) ||
                            t.StartLocation.ToLower().Contains(searchTerm) ||
                            (t.IncludedItems != null && t.IncludedItems.ToLower().Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> HasAvailableSeatsAsync(Guid tripId, int requiredSeats)
        {
            var trip = await _context.Set<Trip>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == tripId);

            return trip != null && trip.AvailableSeats >= requiredSeats;
        }
    }
}

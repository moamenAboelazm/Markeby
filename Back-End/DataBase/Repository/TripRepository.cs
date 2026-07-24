using DataBase.Contexts;
using Library.Enums;
using Library.Features.Trips;
using Library.IRepository;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

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

        public async Task<Trip?> GetTripWithDetailsByIdAsync(Guid id)
        {
            string cacheKey = $"TripDetailsCacheKey_{id}";

            if (!_cache.TryGetValue(cacheKey, out Trip? trip))
            {
                trip = await _context.Set<Trip>().Include(t => t.Boat).ThenInclude(b => b.Images).Include(t => t.Captain).Include(t => t.Bookings)
                    .Include(t => t.Passengers).Include(t => t.Images).AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

                if (trip != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                    _cache.Set(cacheKey, trip, cacheOptions);
                }
            }

            return trip;
        }

        public async Task<IReadOnlyList<Trip>> GetAllTripsWithDetailsAsync()
        {
            string cacheKey = "AllTripsCacheKey";

            if (!_cache.TryGetValue(cacheKey, out IReadOnlyList<Trip>? trips))
            {
                trips = await _context.Set<Trip>().Include(t => t.Boat).Include(t => t.Captain).Include(t => t.Images).AsNoTracking().ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24));

                _cache.Set(cacheKey, trips, cacheOptions);
            }

            return trips ?? new List<Trip>();
        }

        public async Task<bool> HasAvailableSeatsAsync(Guid tripId, int requiredSeats)
        {
            var trip = await _context.Set<Trip>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == tripId);

            return trip != null && trip.AvailableSeats >= requiredSeats;
        }

        public async Task<TripDashboardStatsDto> GetDashboardStatsAsync()
        {
            var stats = await _context.Trips
                .GroupBy(x => 1)
                .Select(g => new TripDashboardStatsDto
                {
                    TotalTrips = g.Count(),
                    ScheduledTrips = g.Count(t => t.Status == TripStatus.Scheduled),
                    OngoingTrips = g.Count(t => t.Status == TripStatus.Ongoing),
                    CompletedTrips = g.Count(t => t.Status == TripStatus.Completed),
                    CancelledTrips = g.Count(t => t.Status == TripStatus.Cancelled)
                }).AsNoTracking().FirstOrDefaultAsync();

            return stats ?? new TripDashboardStatsDto();
        }
    }
}

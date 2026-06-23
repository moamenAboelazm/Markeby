using DataBase.Contexts;
using Library.Enums;
using Library.Features.Captains;
using Library.IRepository;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DataBase.Repository
{
    public class CaptainRepository : GenericRepository<Captain>, ICaptainRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public CaptainRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IReadOnlyList<Captain>> GetAllCaptainsWithDetailsAsync()
        {
            string cacheKey = "AllCaptainsCacheKey";

            if (!_cache.TryGetValue(cacheKey, out IReadOnlyList<Captain>? captains))
            {
                captains = await _context.Set<Captain>().Include(c => c.Boats).AsNoTracking().ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24));

                _cache.Set(cacheKey, captains, cacheOptions);
            }

            return captains ?? new List<Captain>();
        }

        public async Task<Captain?> GetCaptainWithDetailsAsync(Guid id)
        {
            string cacheKey = $"CaptainDetailsCacheKey_{id}";

            if (!_cache.TryGetValue(cacheKey, out Captain? captain))
            {
                captain = await _context.Set<Captain>().Include(c => c.Boats).Include(c => c.Trips).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

                if (captain != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    _cache.Set(cacheKey, captain, cacheOptions);
                }
            }

            return captain;
        }

        public async Task<CaptainDashboardStatsDto> GetDashboardStatsAsync()
        {
            var now = DateTime.UtcNow;

            var total = await _context.Set<Captain>().CountAsync();

            var onMission = await _context.Set<Captain>()
                .CountAsync(c => c.Trips.Any(t =>t.StartTime <= now &&t.EndTime >= now &&t.Status != TripStatus.Cancelled));

            var onShoreLeave = await _context.Set<Captain>()
                .CountAsync(c => c.IsAvailable && !c.Trips.Any(t =>t.StartTime <= now &&t.EndTime >= now &&t.Status != TripStatus.Cancelled));

            return new CaptainDashboardStatsDto
            {
                TotalCaptains = total,
                OnMission = onMission,
                OnShoreLeave = onShoreLeave
            };
        }

        public async Task<bool> IsCaptainAvailableAsync(Guid captainId, DateTime startTime, DateTime endTime)
        {
            var hasConflict = await _context.Set<Trip>()
                .AnyAsync(t => t.CaptainId == captainId && t.Status != TripStatus.Cancelled && t.StartTime < endTime && t.EndTime > startTime);

            return !hasConflict;
        }
    }
}

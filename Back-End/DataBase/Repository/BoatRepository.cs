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
    public class BoatRepository : GenericRepository<Boat>, IBoatRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public BoatRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IReadOnlyList<Boat>> GetActiveBoatsAsync()
        {
            string cacheKey = "ActiveBoatsCacheKey";

            if (!_cache.TryGetValue(cacheKey, out IReadOnlyList<Boat>? activeBoats))
            {
                activeBoats = await _context.Set<Boat>()
                    .Include(b => b.Captains)
                    .Include(b => b.Images)
                    .Where(b => b.Status == BoatStatus.Active)
                    .AsNoTracking()
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12));

                _cache.Set(cacheKey, activeBoats, cacheOptions);
            }

            return activeBoats ?? new List<Boat>();
        }

        public async Task<IReadOnlyList<Boat>> GetAllBoatsAsync()
        {
            string cacheKey = "AllBoatsCacheKey";

            if (!_cache.TryGetValue(cacheKey, out IReadOnlyList<Boat>? allBoats))
            {
                allBoats = await _context.Set<Boat>()
                    .Include(b => b.Captains)
                    .Include(b => b.Images)
                    .AsNoTracking()
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12));

                _cache.Set(cacheKey, allBoats, cacheOptions);
            }

            return allBoats ?? new List<Boat>();
        }

        public async Task<Boat?> GetBoatWithDetailsAsync(Guid id)
        {
            string cacheKey = $"BoatDetailsCacheKey_{id}";

            if (!_cache.TryGetValue(cacheKey, out Boat? boat))
            {
                boat = await _context.Set<Boat>()
                    .Include(b => b.Captains)
                    .Include(b => b.Trips)
                    .Include(b => b.Images)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (boat != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(15));

                    _cache.Set(cacheKey, boat, cacheOptions);
                }
            }

            return boat;
        }

        public async Task<IReadOnlyList<Boat>> GetBoatsByCapacityAsync(int minimumCapacity)
        {
            return await _context.Set<Boat>()
                .Where(b => b.Capacity >= minimumCapacity && b.Status == BoatStatus.Active)
                .Include(b => b.Images)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Boat>> GetBoatsByCaptainIdAsync(Guid captainId)
        {
            return await _context.Set<Boat>()
                .Where(b => b.Captains.Any(c => c.Id == captainId))
                .Include(b => b.Images)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsBoatAvailableAsync(Guid boatId, DateTime startTime, DateTime endTime)
        {
            var hasConflict = await _context.Set<Trip>()
                .AnyAsync(t => t.BoatId == boatId &&
                               t.Status != TripStatus.Cancelled &&
                               t.StartTime < endTime &&
                               t.EndTime > startTime);

            return !hasConflict;
        }
    }
}

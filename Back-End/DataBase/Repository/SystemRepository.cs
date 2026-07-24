using AutoMapper;
using DataBase.Contexts;
using DataBase.Repository;
using Library.Enums;
using Library.Features.Boats;
using Library.Features.Trips;
using Library.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository
{
    public class DashboardRepository(AppDbContext _context, IMapper _mapper) : IDashboardRepository
    {
        public async Task<SystemDashboardStatsDto> GetSystemDashboardStatsAsync()
        {
            var totalTrips = await _context.Trips.CountAsync();
            var totalCaptains = await _context.Captains.CountAsync();
            var totalBoats = await _context.Boats.CountAsync();
            var totalBookings = await _context.Bookings.CountAsync();

            var canceledBookings = await _context.Bookings
                .CountAsync(b => !string.IsNullOrEmpty(b.CancellationReason));

            var totalEarnings = await _context.Bookings
                .Where(b => string.IsNullOrEmpty(b.CancellationReason)).SumAsync(b => b.TotalPrice);

            var totalCompletedTrips = await _context.Trips.CountAsync(t => t.Status == TripStatus.Completed);
            var totalCancelledTrips = await _context.Trips.CountAsync(t => t.Status == TripStatus.Cancelled);
            var totalScheduledTrips = await _context.Trips.CountAsync(t => t.Status == TripStatus.Scheduled);

            var recentBoats = await _context.Boats.AsNoTracking().OrderByDescending(b => b.Id).Take(10).ToListAsync();

            var recentTrips = await _context.Trips.Include(t => t.Boat).Include(t => t.Captain).AsNoTracking()
                .OrderByDescending(t => t.StartTime).Take(10).ToListAsync();

            return new SystemDashboardStatsDto
            {
                TotalBookings = totalBookings,
                TotalTrips = totalTrips,
                TotalCaptains = totalCaptains,
                TotalBoats = totalBoats,
                TotalEarnings = totalEarnings,
                CanceledBookings = canceledBookings,

                Boats = _mapper.Map<IEnumerable<DtoBoat>>(recentBoats),
                Trips = _mapper.Map<IEnumerable<DtoTrip>>(recentTrips)
            };
        }
    }
}
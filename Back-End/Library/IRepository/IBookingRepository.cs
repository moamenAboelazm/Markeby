using Library.Features.Bookings;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.IRepository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IReadOnlyList<Booking>> GetAllBookingsWithDetailsAsync();
        Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId);
        Task<SystemDashboardDto> GetSystemDashboardAsync();
        Task<TripDashboardDto?> GetTripDashboardAsync(Guid tripId);
        Task<bool> HasUserBookedTripAsync(string userId, Guid tripId);
    }
}
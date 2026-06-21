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
        Task<IReadOnlyList<Booking>> GetUserBookingsAsync(string userId);
        Task<IReadOnlyList<Booking>> GetBookingsByTripIdAsync(Guid tripId);
        Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId);
        Task<IReadOnlyList<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalRevenueByTripIdAsync(Guid tripId);
        Task<bool> HasUserBookedTripAsync(string userId, Guid tripId);
    }
}

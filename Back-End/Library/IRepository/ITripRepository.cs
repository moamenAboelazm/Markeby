using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.IRepository
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        Task<IReadOnlyList<Trip>> GetAvailableUpcomingTripsAsync();
        Task<Trip?> GetTripWithDetailsByIdAsync(Guid id);
        Task<IReadOnlyList<Trip>> GetTripsByBoatIdAsync(Guid boatId);
        Task<IReadOnlyList<Trip>> GetTripsByCaptainIdAsync(Guid captainId);
        Task<IReadOnlyList<Trip>> GetTripsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IReadOnlyList<Trip>> SearchTripsAsync(string searchTerm);
        Task<bool> HasAvailableSeatsAsync(Guid tripId, int requiredSeats);
    }
}

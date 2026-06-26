using Library.Features.Boats;
using Library.Features.Trips;
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
        Task<Trip?> GetTripWithDetailsByIdAsync(Guid id);
        Task<IReadOnlyList<Trip>> GetAllTripsWithDetailsAsync();
        Task<bool> HasAvailableSeatsAsync(Guid tripId, int requiredSeats);
        Task<TripDashboardStatsDto> GetDashboardStatsAsync();
    }
}

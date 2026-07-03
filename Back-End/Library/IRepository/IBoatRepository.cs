using Library.Features.Boats;
using Library.Models;

namespace Library.IRepository
{
    public interface IBoatRepository : IGenericRepository<Boat>
    {
        Task<IReadOnlyList<Boat>> GetAllBoatsAsync();
        Task<Boat?> GetBoatWithDetailsAsync(Guid id);
        Task<Boat?> GetBoatForUpdateAsync(Guid id);
        Task<IReadOnlyList<Boat>> GetBoatsByCaptainIdAsync(Guid captainId);
        Task<BoatDashboardStatsDto> GetDashboardStatsAsync();
        Task<bool> IsBoatAvailableAsync(Guid boatId, DateTime startTime, DateTime endTime);
        Task<bool> IsBoatAvailableForUpdateTripAsync(Guid boatId, Guid tripId , DateTime startTime, DateTime endTime);
    }
}

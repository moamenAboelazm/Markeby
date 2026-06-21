using Library.Models;

namespace Library.IRepository
{
    public interface IBoatRepository : IGenericRepository<Boat>
    {
        Task<IReadOnlyList<Boat>> GetActiveBoatsAsync();
        Task<Boat?> GetBoatWithDetailsAsync(Guid id);
        Task<IReadOnlyList<Boat>> GetBoatsByCapacityAsync(int minimumCapacity);
        Task<IReadOnlyList<Boat>> GetBoatsByCaptainIdAsync(Guid captainId);
        Task<bool> IsBoatAvailableAsync(Guid boatId, DateTime startTime, DateTime endTime);
    }
}

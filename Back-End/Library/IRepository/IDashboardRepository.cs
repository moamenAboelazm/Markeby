
namespace Library.IRepository
{
    public interface IDashboardRepository 
    {
        Task<SystemDashboardStatsDto> GetSystemDashboardStatsAsync();
    }
}
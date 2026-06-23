using Library.Features.Captains;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.IRepository
{
    public interface ICaptainRepository : IGenericRepository<Captain>
    {
        Task<IReadOnlyList<Captain>> GetAllCaptainsWithDetailsAsync();
        Task<Captain?> GetCaptainWithDetailsAsync(Guid id);
        Task<CaptainDashboardStatsDto> GetDashboardStatsAsync();
        Task<bool> IsCaptainAvailableAsync(Guid captainId, DateTime startTime, DateTime endTime);
    }
}

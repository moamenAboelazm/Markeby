using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dashboards.Captains
{
    public class GetCaptainDashboardStatsQuery : IRequest<CaptainDashboardStatsDto>
    {
    }

    public class GetCaptainDashboardStatsQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetCaptainDashboardStatsQuery, CaptainDashboardStatsDto>
    {
        public async Task<CaptainDashboardStatsDto> Handle(GetCaptainDashboardStatsQuery data, CancellationToken cancellationToken)
        {
            var captains = await _unitOfWork.Captains.GetAllCaptainsWithDetailsAsync();
            var now = DateTime.UtcNow;

            var total = captains.Count;
            var onMission = captains.Count(c => c.Trips != null && c.Trips.Any(t => t.StartTime <= now && t.EndTime >= now));
            var onShoreLeave = captains.Count(c => c.IsAvailable && !(c.Trips != null && c.Trips.Any(t => t.StartTime <= now && t.EndTime >= now)));

            return new CaptainDashboardStatsDto
            {
                TotalCaptains = total,
                OnMission = onMission,
                OnShoreLeave = onShoreLeave
            };
        }
    }
}

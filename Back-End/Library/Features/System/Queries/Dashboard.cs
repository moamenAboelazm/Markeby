using Library.IRepository;
using MediatR;

namespace Library.Features.Dashboard.Queries
{
    public class GetSystemDashboardStatsQuery : IRequest<SystemDashboardStatsDto>
    {
    }

    public class GetSystemDashboardStatsQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetSystemDashboardStatsQuery, SystemDashboardStatsDto>
    {
        public async Task<SystemDashboardStatsDto> Handle(GetSystemDashboardStatsQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Dashboard.GetSystemDashboardStatsAsync();
        }
    }
}
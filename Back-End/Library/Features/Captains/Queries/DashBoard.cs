using Library.IRepository;
using MediatR;
namespace Library.Features.Captains.Queries
{
    public class GetCaptainDashboardStatsQuery : IRequest<CaptainDashboardStatsDto>
    {
    }

    public class GetCaptainDashboardStatsQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetCaptainDashboardStatsQuery, CaptainDashboardStatsDto>
    {
        public async Task<CaptainDashboardStatsDto> Handle(GetCaptainDashboardStatsQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Captains.GetDashboardStatsAsync();
        }
    }
}
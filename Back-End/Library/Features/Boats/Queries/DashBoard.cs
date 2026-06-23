using Library.Enums;
using Library.IRepository;
using MediatR;

namespace Library.Features.Boats.Queries
{
    public class GetBoatDashboardStatsQuery : IRequest<BoatDashboardStatsDto>
    {
    }

    public class GetBoatDashboardStatsQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetBoatDashboardStatsQuery, BoatDashboardStatsDto>
    {
        public async Task<BoatDashboardStatsDto> Handle(GetBoatDashboardStatsQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Boats.GetDashboardStatsAsync();
        }
    }
}
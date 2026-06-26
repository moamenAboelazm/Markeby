using Library.Features.Boats;
using Library.IRepository;
using MediatR;

namespace Library.Features.Trips.Queries
{
    public class GetTripDashboardStatsQuery : IRequest<TripDashboardStatsDto>
    {
    }

    public class GetTripDashboardStatsQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTripDashboardStatsQuery, TripDashboardStatsDto>
    {
        public async Task<TripDashboardStatsDto> Handle(GetTripDashboardStatsQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Trips.GetDashboardStatsAsync();
        }
    }
}

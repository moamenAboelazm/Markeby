using Library.IRepository;
using MediatR;

namespace Library.Features.Bookings.Queries
{
    public class GetTripDashboardQuery : IRequest<TripDashboardDto?>
    {
        public Guid TripId { get; set; }
    }

    public class GetTripDashboardQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTripDashboardQuery, TripDashboardDto?>
    {
        public async Task<TripDashboardDto?> Handle(GetTripDashboardQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Bookings.GetTripDashboardAsync(request.TripId);
        }
    }
}
using Library.IRepository;
using MediatR;

namespace Library.Features.Bookings.Queries
{
    public class GetTotalRevenueByTripIdQuery : IRequest<decimal>
    {
        public Guid TripId { get; set; }
    }

    public class GetTotalRevenueByTripIdQueryHandler : IRequestHandler<GetTotalRevenueByTripIdQuery, decimal>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalRevenueByTripIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> Handle(GetTotalRevenueByTripIdQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Bookings.GetTotalRevenueByTripIdAsync(data.TripId);
        }
    }
}

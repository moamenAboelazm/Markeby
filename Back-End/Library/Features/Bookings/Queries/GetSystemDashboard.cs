using Library.IRepository;
using MediatR;

namespace Library.Features.Bookings.Queries
{
    public class GetSystemDashboardQuery : IRequest<SystemDashboardDto>
    {
    }

    public class GetSystemDashboardQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetSystemDashboardQuery, SystemDashboardDto>
    {
        public async Task<SystemDashboardDto> Handle(GetSystemDashboardQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Bookings.GetSystemDashboardAsync();
        }
    }
}
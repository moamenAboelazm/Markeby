using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;

namespace Library.Features.Trips.Queries
{
    public class GetPagedAvailableTripsQuery : IRequest<PagedResult<DtoTripforUser>>
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class GetPagedAvailableTripsQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedAvailableTripsQuery, PagedResult<DtoTripforUser>>
    {
        public async Task<PagedResult<DtoTripforUser>> Handle(GetPagedAvailableTripsQuery data, CancellationToken cancellationToken)
        {
            var allTrips = await _unitOfWork.Trips.GetAllTripsWithDetailsAsync();
            var query = allTrips.AsEnumerable();

            var now = DateTime.UtcNow.AddHours(3);
            query = query.Where(t => t.StartTime > now &&t.Status == TripStatus.Scheduled &&t.AvailableSeats > 0);

            if (!string.IsNullOrWhiteSpace(data.SearchTerm))
            {
                var term = data.SearchTerm.ToLower();
                query = query.Where(t => (t.Title != null && t.Title.ToLower().Contains(term)) || (t.StartLocation != null && t.StartLocation.ToLower().Contains(term)));
            }

            query = query.OrderBy(t => t.StartTime);

            var totalCount = query.Count();

            var pagedEntities = query.Skip((data.PageNumber - 1) * data.PageSize).Take(data.PageSize).ToList();

            var mappedItems = _mapper.Map<List<DtoTripforUser>>(pagedEntities);

            return new PagedResult<DtoTripforUser>
            {
                Items = mappedItems,
                TotalCount = totalCount,
                PageNumber = data.PageNumber,
                PageSize = data.PageSize
            };
        }
    }
}
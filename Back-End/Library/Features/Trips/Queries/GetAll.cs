using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;

namespace Library.Features.Trips.Queries
{
    public class GetPagedTripsQuery : IRequest<PagedResult<DtoTrip>>
    {
        public string? SearchTerm { get; set; }
        public bool OnlyAvailableUpcoming { get; set; }
        public string? BoatName { get; set; }
        public string? CaptainName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool SortDescending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllTripsQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedTripsQuery, PagedResult<DtoTrip>>
    {
        public async Task<PagedResult<DtoTrip>> Handle(GetPagedTripsQuery data, CancellationToken cancellationToken)
        {
            var allTrips = await _unitOfWork.Trips.GetAllTripsWithDetailsAsync();
            var query = allTrips.AsEnumerable();

            if (data.OnlyAvailableUpcoming)
            {
                var now = DateTime.UtcNow.AddHours(3);
                query = query.Where(t => t.StartTime > now && t.Status == TripStatus.Scheduled && t.AvailableSeats > 0);
            }

            if (!string.IsNullOrWhiteSpace(data.BoatName))
            {
                var boatName = data.BoatName.ToLower();
                query = query.Where(t => t.Boat != null && t.Boat.Name != null && t.Boat.Name.ToLower().Contains(boatName));
            }

            if (!string.IsNullOrWhiteSpace(data.CaptainName))
            {
                var captainName = data.CaptainName.ToLower();
                query = query.Where(t => t.Captain != null && t.Captain.FullName != null && t.Captain.FullName.ToLower().Contains(captainName));
            }

            if (data.StartDate.HasValue)
                query = query.Where(t => t.StartTime >= data.StartDate.Value);

            if (data.EndDate.HasValue)
                query = query.Where(t => t.EndTime <= data.EndDate.Value);

            if (!string.IsNullOrWhiteSpace(data.SearchTerm))
            {
                var term = data.SearchTerm.ToLower();
                query = query.Where(t => (t.Title != null && t.Title.ToLower().Contains(term)) || (t.StartLocation != null && t.StartLocation.ToLower().Contains(term)));
            }

            query = data.SortDescending ? query.OrderByDescending(t => t.StartTime) : query.OrderBy(t => t.StartTime);

            var totalCount = query.Count();

            var pagedEntities = query.Skip((data.PageNumber - 1) * data.PageSize).Take(data.PageSize).ToList();

            var mappedItems = _mapper.Map<List<DtoTrip>>(pagedEntities);

            return new PagedResult<DtoTrip>
            {
                Items = mappedItems,
                TotalCount = totalCount,
                PageNumber = data.PageNumber,
                PageSize = data.PageSize
            };
        }
    }
}
using AutoMapper;
using Library.IRepository;
using Library.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Trips.Queries
{
    public class GetAllTripsQuery : IRequest<PagedResult<DtoTrip>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public Guid? BoatId { get; set; }
        public Guid? CaptainId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public bool? OnlyAvailableUpcoming { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
    public class GetAllTripsQueryHandler : IRequestHandler<GetAllTripsQuery, PagedResult<DtoTrip>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTripsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<DtoTrip>> Handle(GetAllTripsQuery data, CancellationToken cancellationToken)
        {
            var trips = await _unitOfWork.Trips.GetAllAsync();
            var queryableTrips = trips.AsQueryable();

            if (!string.IsNullOrEmpty(data.SearchTerm))
            {
                queryableTrips = queryableTrips.Where(t =>
                    t.Title.Contains(data.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    t.StartLocation.Contains(data.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (data.BoatId.HasValue)
                queryableTrips = queryableTrips.Where(t => t.BoatId == data.BoatId.Value);
            

            if (data.CaptainId.HasValue)
                queryableTrips = queryableTrips.Where(t => t.CaptainId == data.CaptainId.Value);
            

            if (data.StartDate.HasValue)
                queryableTrips = queryableTrips.Where(t => t.StartTime >= data.StartDate.Value);
            

            if (data.EndDate.HasValue)
                queryableTrips = queryableTrips.Where(t => t.EndTime <= data.EndDate.Value);
            

            if (!string.IsNullOrEmpty(data.Status))
                queryableTrips = queryableTrips.Where(t => t.Status.ToString().Equals(data.Status, StringComparison.OrdinalIgnoreCase));
            

            if (!string.IsNullOrEmpty(data.Type))
                queryableTrips = queryableTrips.Where(t => t.Type.ToString().Equals(data.Type, StringComparison.OrdinalIgnoreCase));
          

            if (data.OnlyAvailableUpcoming.HasValue && data.OnlyAvailableUpcoming.Value)
                queryableTrips = queryableTrips.Where(t => t.StartTime > DateTime.UtcNow && t.AvailableSeats > 0);
            

            if (!string.IsNullOrEmpty(data.SortBy))
            {
                queryableTrips = data.SortBy.ToLower() switch
                {
                    "title" => data.SortDescending ? queryableTrips.OrderByDescending(t => t.Title) : queryableTrips.OrderBy(t => t.Title),
                    "price" => data.SortDescending ? queryableTrips.OrderByDescending(t => t.Price) : queryableTrips.OrderBy(t => t.Price),
                    "starttime" => data.SortDescending ? queryableTrips.OrderByDescending(t => t.StartTime) : queryableTrips.OrderBy(t => t.StartTime),
                    "availableseats" => data.SortDescending ? queryableTrips.OrderByDescending(t => t.AvailableSeats) : queryableTrips.OrderBy(t => t.AvailableSeats),
                    _ => data.SortDescending ? queryableTrips.OrderByDescending(t => t.Id) : queryableTrips.OrderBy(t => t.Id)
                };
            }
            else
                queryableTrips = data.SortDescending ? queryableTrips.OrderByDescending(t => t.StartTime) : queryableTrips.OrderBy(t => t.StartTime);
            

            var totalCount = queryableTrips.Count();
            var pagedTrips = queryableTrips.Skip((data.PageNumber - 1) * data.PageSize).Take(data.PageSize).ToList();

            return new PagedResult<DtoTrip>
            {
                Items = _mapper.Map<List<DtoTrip>>(pagedTrips),
                TotalCount = totalCount,
                PageNumber = data.PageNumber,
                PageSize = data.PageSize
            };
        }
    }
}

using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Bookings.Queries
{
    public class GetAllBookingsQuery : IRequest<PagedResult<DtoBooking>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? UserId { get; set; }
        public Guid? TripId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? OnlyPastTrips { get; set; }

        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }

    public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, PagedResult<DtoBooking>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<DtoBooking>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _unitOfWork.Bookings.GetAllAsync();
            var queryableBookings = bookings.AsQueryable();

            if (!string.IsNullOrEmpty(request.UserId))
                queryableBookings = queryableBookings.Where(b => b.UserId == request.UserId);
            
            if (request.TripId.HasValue)
                queryableBookings = queryableBookings.Where(b => b.TripId == request.TripId.Value);
            
            if (request.StartDate.HasValue)
                queryableBookings = queryableBookings.Where(b => b.BookingDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                queryableBookings = queryableBookings.Where(b => b.BookingDate <= request.EndDate.Value);
            
            if (request.OnlyPastTrips.HasValue && request.OnlyPastTrips.Value)
                queryableBookings = queryableBookings.Where(b => b.Trip != null && (b.Trip.Status == TripStatus.Completed || b.Trip.EndTime < DateTime.UtcNow));
            
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                queryableBookings = request.SortBy.ToLower() switch
                {
                    "price" => request.SortDescending ? queryableBookings.OrderByDescending(b => b.Trip != null ? b.Trip.Price : 0) : queryableBookings.OrderBy(b => b.Trip != null ? b.Trip.Price : 0),
                    "starttime" => request.SortDescending ? queryableBookings.OrderByDescending(b => b.Trip != null ? b.Trip.StartTime : DateTime.MinValue) : queryableBookings.OrderBy(b => b.Trip != null ? b.Trip.StartTime : DateTime.MinValue),
                    "availableseats" => request.SortDescending ? queryableBookings.OrderByDescending(b => b.Trip != null ? b.Trip.AvailableSeats : 0) : queryableBookings.OrderBy(b => b.Trip != null ? b.Trip.AvailableSeats : 0),
                    "type" => request.SortDescending ? queryableBookings.OrderByDescending(b => b.Trip != null ? b.Trip.Type : 0) : queryableBookings.OrderBy(b => b.Trip != null ? b.Trip.Type : 0),
                    "status" => request.SortDescending ? queryableBookings.OrderByDescending(b => b.Trip != null ? b.Trip.Status : 0) : queryableBookings.OrderBy(b => b.Trip != null ? b.Trip.Status : 0),
                    _ => request.SortDescending ? queryableBookings.OrderByDescending(b => b.BookingDate) : queryableBookings.OrderBy(b => b.BookingDate)
                };
            }
            else
                queryableBookings = request.SortDescending ? queryableBookings.OrderByDescending(b => b.BookingDate) : queryableBookings.OrderBy(b => b.BookingDate);
            

            var totalCount = queryableBookings.Count();
            var pagedBookings = queryableBookings.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

            return new PagedResult<DtoBooking>
            {
                Items = _mapper.Map<List<DtoBooking>>(pagedBookings),
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}

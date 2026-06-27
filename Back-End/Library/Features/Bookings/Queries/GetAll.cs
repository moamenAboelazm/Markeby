using AutoMapper;
using Library.IRepository;
using Library.Models;
using MediatR;

namespace Library.Features.Bookings.Queries
{
    public class GetPagedBookingsQuery : IRequest<PagedResult<DtoBooking>>
    {
        public string? UserId { get; set; }
        public Guid? TripId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool OnlyActiveBookings { get; set; }
        public bool SortDescending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetPagedBookingsQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedBookingsQuery, PagedResult<DtoBooking>>
    {
        public async Task<PagedResult<DtoBooking>> Handle(GetPagedBookingsQuery data, CancellationToken cancellationToken)
        {
            var allBookings = await _unitOfWork.Bookings.GetAllBookingsWithDetailsAsync();
            var query = allBookings.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(data.UserId))
                query = query.Where(b => b.UserId == data.UserId);

            if (data.TripId.HasValue)
                query = query.Where(b => b.TripId == data.TripId.Value);

            if (data.StartDate.HasValue)
                query = query.Where(b => b.BookingDate >= data.StartDate.Value);

            if (data.EndDate.HasValue)
                query = query.Where(b => b.BookingDate <= data.EndDate.Value);

            if (data.OnlyActiveBookings)
                query = query.Where(b => string.IsNullOrEmpty(b.CancellationReason));

            query = data.SortDescending ? query.OrderByDescending(b => b.BookingDate) : query.OrderBy(b => b.BookingDate);

            var totalCount = query.Count();
            var pagedEntities = query.Skip((data.PageNumber - 1) * data.PageSize).Take(data.PageSize).ToList();
            var mappedItems = _mapper.Map<List<DtoBooking>>(pagedEntities);

            return new PagedResult<DtoBooking>
            {
                Items = mappedItems,
                TotalCount = totalCount,
                PageNumber = data.PageNumber,
                PageSize = data.PageSize
            };
        }
    }
}
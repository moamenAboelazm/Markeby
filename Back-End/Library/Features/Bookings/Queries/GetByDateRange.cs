using AutoMapper;
using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Bookings.Queries
{
    public class GetBookingsByDateRangeQuery : IRequest<IReadOnlyList<DtoBooking>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetBookingsByDateRangeQueryHandler : IRequestHandler<GetBookingsByDateRangeQuery, IReadOnlyList<DtoBooking>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookingsByDateRangeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoBooking>> Handle(GetBookingsByDateRangeQuery data, CancellationToken cancellationToken)
        {
            var bookings = await _unitOfWork.Bookings.GetBookingsByDateRangeAsync(data.StartDate, data.EndDate);
            return _mapper.Map<IReadOnlyList<DtoBooking>>(bookings);
        }
    }
}

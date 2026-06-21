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
    public class GetBookingsByTripIdQuery : IRequest<IReadOnlyList<DtoBooking>>
    {
        public Guid TripId { get; set; }
    }

    public class GetBookingsByTripIdQueryHandler : IRequestHandler<GetBookingsByTripIdQuery, IReadOnlyList<DtoBooking>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookingsByTripIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoBooking>> Handle(GetBookingsByTripIdQuery data, CancellationToken cancellationToken)
        {
            var bookings = await _unitOfWork.Bookings.GetBookingsByTripIdAsync(data.TripId);
            return _mapper.Map<IReadOnlyList<DtoBooking>>(bookings);
        }
    }
}

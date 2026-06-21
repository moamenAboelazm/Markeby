using AutoMapper;
using Library.IRepository;
using MediatR;

namespace Library.Features.Bookings.Queries
{
    public class GetBookingWithDetailsQuery : IRequest<DtoBooking?>
    {
        public Guid Id { get; set; }
    }

    public class GetBookingWithDetailsQueryHandler : IRequestHandler<GetBookingWithDetailsQuery, DtoBooking?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookingWithDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DtoBooking?> Handle(GetBookingWithDetailsQuery data, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(data.Id);
            return booking == null ? null : _mapper.Map<DtoBooking>(booking);
        }
    }
}

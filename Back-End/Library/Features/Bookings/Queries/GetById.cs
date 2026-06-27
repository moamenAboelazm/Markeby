using AutoMapper;
using Library.IRepository;
using MediatR;

namespace Library.Features.Bookings.Queries
{
    public class GetBookingWithDetailsQuery : IRequest<DtoBooking?>
    {
        public Guid Id { get; set; }
    }

    public class GetBookingWithDetailsQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetBookingWithDetailsQuery, DtoBooking?>
    {
        public async Task<DtoBooking?> Handle(GetBookingWithDetailsQuery data, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(data.Id);
            return booking == null ? null : _mapper.Map<DtoBooking>(booking);
        }
    }
}

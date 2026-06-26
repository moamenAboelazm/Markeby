using AutoMapper;
using Library.IRepository;
using MediatR;

namespace Library.Features.Trips.Queries
{
    public class GetTripWithDetailsByIdQuery : IRequest<DtoTrip?>
    {
        public Guid Id { get; set; }
    }

    public class GetTripWithDetailsByIdQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetTripWithDetailsByIdQuery, DtoTrip?>
    {
        public async Task<DtoTrip?> Handle(GetTripWithDetailsByIdQuery data, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.Trips.GetTripWithDetailsByIdAsync(data.Id);
            return trip == null ? null : _mapper.Map<DtoTrip>(trip);
        }
    }
}

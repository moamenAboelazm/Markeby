using AutoMapper;
using Library.IRepository;
using MediatR;
namespace Library.Features.Boats.Queries
{
    public class GetBoatByIdQuery : IRequest<DtoBoat?>
    {
        public Guid Id { get; set; }
    }

    public class GetBoatByIdQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetBoatByIdQuery, DtoBoat?>
    {
        public async Task<DtoBoat?> Handle(GetBoatByIdQuery data, CancellationToken cancellationToken)
        {
            var boat = await _unitOfWork.Boats.GetBoatWithDetailsAsync(data.Id);

            if (boat == null) return null;

            var mappedBoat = _mapper.Map<DtoBoat>(boat);
            var trips = _mapper.Map<IReadOnlyList<DtoBoatTripsTable>>(boat.Trips);

            mappedBoat.Trips = trips;

            return mappedBoat;
        }
    }
}

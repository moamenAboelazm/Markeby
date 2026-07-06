using AutoMapper;
using Library.IRepository;
using MediatR;

namespace Library.Features.Captains.Queries
{
    public class GetCaptainWithBoatsAndTripsQuery : IRequest<DtoCaptain?>
    {
        public Guid Id { get; set; }
    }
    public class GetCaptainWithBoatsAndTripsQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetCaptainWithBoatsAndTripsQuery, DtoCaptain?>
    {
        public async Task<DtoCaptain?> Handle(GetCaptainWithBoatsAndTripsQuery data, CancellationToken cancellationToken)
        {
            var captain = await _unitOfWork.Captains.GetCaptainWithDetailsAsync(data.Id);
            if (captain == null) return null;

            var trips = _mapper.Map<IReadOnlyList<DtoCaptainTripsTable>>(captain.Trips);
            
            var mappedCaptain = _mapper.Map<DtoCaptain>(captain);

            mappedCaptain.TripsTable = trips;

            return mappedCaptain;
        }
    }
}

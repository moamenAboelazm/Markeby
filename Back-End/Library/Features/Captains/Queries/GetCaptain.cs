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
            var captain = await _unitOfWork.Captains.GetCaptainWithBoatsAndTripsAsync(data.Id);
            return captain == null ? null : _mapper.Map<DtoCaptain>(captain);
        }
    }
}

using AutoMapper;
using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Trips.Queries
{
    public class GetTripsByBoatIdQuery : IRequest<IReadOnlyList<DtoTrip>>
    {
        public Guid BoatId { get; set; }
    }

    public class GetTripsByBoatIdQueryHandler : IRequestHandler<GetTripsByBoatIdQuery, IReadOnlyList<DtoTrip>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTripsByBoatIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoTrip>> Handle(GetTripsByBoatIdQuery data, CancellationToken cancellationToken)
        {
            var trips = await _unitOfWork.Trips.GetTripsByBoatIdAsync(data.BoatId);
            return _mapper.Map<IReadOnlyList<DtoTrip>>(trips);
        }
    }
}

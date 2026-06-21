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
    public class GetTripsByCaptainIdQuery : IRequest<IReadOnlyList<DtoTrip>>
    {
        public Guid CaptainId { get; set; }
    }

    public class GetTripsByCaptainIdQueryHandler : IRequestHandler<GetTripsByCaptainIdQuery, IReadOnlyList<DtoTrip>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTripsByCaptainIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoTrip>> Handle(GetTripsByCaptainIdQuery data, CancellationToken cancellationToken)
        {
            var trips = await _unitOfWork.Trips.GetTripsByCaptainIdAsync(data.CaptainId);
            return _mapper.Map<IReadOnlyList<DtoTrip>>(trips);
        }
    }
}

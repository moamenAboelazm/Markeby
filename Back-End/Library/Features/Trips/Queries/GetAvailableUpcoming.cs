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
    public class GetAvailableUpcomingTripsQuery : IRequest<IReadOnlyList<DtoTrip>>
    {
    }

    public class GetAvailableUpcomingTripsQueryHandler : IRequestHandler<GetAvailableUpcomingTripsQuery, IReadOnlyList<DtoTrip>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAvailableUpcomingTripsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoTrip>> Handle(GetAvailableUpcomingTripsQuery data, CancellationToken cancellationToken)
        {
            var trips = await _unitOfWork.Trips.GetAvailableUpcomingTripsAsync();
            return _mapper.Map<IReadOnlyList<DtoTrip>>(trips);
        }
    }
}

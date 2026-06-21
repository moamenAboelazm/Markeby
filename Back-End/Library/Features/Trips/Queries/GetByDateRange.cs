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
    public class GetTripsByDateRangeQuery : IRequest<IReadOnlyList<DtoTrip>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetTripsByDateRangeQueryHandler : IRequestHandler<GetTripsByDateRangeQuery, IReadOnlyList<DtoTrip>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTripsByDateRangeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoTrip>> Handle(GetTripsByDateRangeQuery data, CancellationToken cancellationToken)
        {
            var trips = await _unitOfWork.Trips.GetTripsByDateRangeAsync(data.StartDate, data.EndDate);
            return _mapper.Map<IReadOnlyList<DtoTrip>>(trips);
        }
    }
}

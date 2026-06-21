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
    public class SearchTripsQuery : IRequest<IReadOnlyList<DtoTrip>>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }

    public class SearchTripsQueryHandler : IRequestHandler<SearchTripsQuery, IReadOnlyList<DtoTrip>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchTripsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoTrip>> Handle(SearchTripsQuery request, CancellationToken cancellationToken)
        {
            var trips = await _unitOfWork.Trips.SearchTripsAsync(request.SearchTerm);
            return _mapper.Map<IReadOnlyList<DtoTrip>>(trips);
        }
    }
}

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
    public class GetTripWithDetailsByIdQuery : IRequest<DtoTrip?>
    {
        public Guid Id { get; set; }
    }

    public class GetTripWithDetailsByIdQueryHandler : IRequestHandler<GetTripWithDetailsByIdQuery, DtoTrip?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTripWithDetailsByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DtoTrip?> Handle(GetTripWithDetailsByIdQuery data, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.Trips.GetTripWithDetailsByIdAsync(data.Id);
            return trip == null ? null : _mapper.Map<DtoTrip>(trip);
        }
    }
}

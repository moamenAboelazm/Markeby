using AutoMapper;
using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Captains.Queries
{
    public class GetCaptainWithBoatsAndTripsQuery : IRequest<DtoCaptain?>
    {
        public Guid Id { get; set; }
    }
    public class GetCaptainWithBoatsAndTripsQueryHandler : IRequestHandler<GetCaptainWithBoatsAndTripsQuery, DtoCaptain?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCaptainWithBoatsAndTripsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DtoCaptain?> Handle(GetCaptainWithBoatsAndTripsQuery request, CancellationToken cancellationToken)
        {
            var captain = await _unitOfWork.Captains.GetCaptainWithBoatsAndTripsAsync(request.Id);
            return captain == null ? null : _mapper.Map<DtoCaptain>(captain);
        }
    }
}

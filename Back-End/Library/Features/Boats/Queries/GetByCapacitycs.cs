using AutoMapper;
using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Boats.Queries
{
    public class GetBoatsByCapacityQuery : IRequest<IReadOnlyList<DtoBoat>>
    {
        public int MinimumCapacity { get; set; }
    }

    public class GetBoatsByCapacityQueryHandler : IRequestHandler<GetBoatsByCapacityQuery, IReadOnlyList<DtoBoat>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBoatsByCapacityQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoBoat>> Handle(GetBoatsByCapacityQuery data, CancellationToken cancellationToken)
        {
            var boats = await _unitOfWork.Boats.GetBoatsByCapacityAsync(data.MinimumCapacity);
            return _mapper.Map<IReadOnlyList<DtoBoat>>(boats);
        }
    }
}

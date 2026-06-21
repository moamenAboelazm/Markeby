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
    public class GetAllActiveBoatsQuery : IRequest<IReadOnlyList<DtoBoat>>
    {

    }
    public class GetAllActiveBoatsQueryHandler : IRequestHandler<GetAllActiveBoatsQuery, IReadOnlyList<DtoBoat>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllActiveBoatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoBoat>> Handle(GetAllActiveBoatsQuery data, CancellationToken cancellationToken)
        {
            var boats = await _unitOfWork.Boats.GetActiveBoatsAsync();
            return _mapper.Map<IReadOnlyList<DtoBoat>>(boats);
        }
    }
}

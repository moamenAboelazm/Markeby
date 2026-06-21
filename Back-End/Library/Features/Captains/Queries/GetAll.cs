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
    public class GetAllCaptainsWithDetailsQuery : IRequest<IReadOnlyList<DtoCaptain>>
    {
    }

    public class GetAllCaptainsWithDetailsQueryHandler : IRequestHandler<GetAllCaptainsWithDetailsQuery, IReadOnlyList<DtoCaptain>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCaptainsWithDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoCaptain>> Handle(GetAllCaptainsWithDetailsQuery request, CancellationToken cancellationToken)
        {
            var captains = await _unitOfWork.Captains.GetAllCaptainsWithDetailsAsync();
            return _mapper.Map<IReadOnlyList<DtoCaptain>>(captains);
        }
    }
}

using AutoMapper;
using Library.IRepository;
using MediatR;

namespace Library.Features.Captains.Queries
{
    public class GetAllCaptainsWithDetailsQuery : IRequest<IReadOnlyList<DtoCaptain>>
    {
    }

    public class GetAllCaptainsWithDetailsQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllCaptainsWithDetailsQuery, IReadOnlyList<DtoCaptain>>
    {
        public async Task<IReadOnlyList<DtoCaptain>> Handle(GetAllCaptainsWithDetailsQuery data, CancellationToken cancellationToken)
        {
            var captains = await _unitOfWork.Captains.GetAllCaptainsWithDetailsAsync();
            return _mapper.Map<IReadOnlyList<DtoCaptain>>(captains);
        }
    }
}

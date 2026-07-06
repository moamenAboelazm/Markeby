using AutoMapper;
using Library.IRepository;
using MediatR;
namespace Library.Features.Boats.Queries
{
    public class GetBoatByIdQuery : IRequest<DtoBoat?>
    {
        public Guid Id { get; set; }
    }

    public class GetBoatByIdQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetBoatByIdQuery, DtoBoat?>
    {
        public async Task<DtoBoat?> Handle(GetBoatByIdQuery data, CancellationToken cancellationToken)
        {
            var boat = await _unitOfWork.Boats.GetBoatWithDetailsAsync(data.Id);

            if (boat == null) return null;

            return _mapper.Map<DtoBoat>(boat);
        }
    }
}

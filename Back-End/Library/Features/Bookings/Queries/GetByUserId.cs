using AutoMapper;
using Library.Features.Bookings.Commands;
using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Bookings.Queries
{
    public class GetUserBookingsQuery : IRequest<IReadOnlyList<DtoBooking>>
    {
        public string UserId { get; set; }
    }

    public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, IReadOnlyList<DtoBooking>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DtoBooking>> Handle(GetUserBookingsQuery data, CancellationToken cancellationToken)
        {
            var bookings = await _unitOfWork.Bookings.GetUserBookingsAsync(data.UserId);
            return _mapper.Map<IReadOnlyList<DtoBooking>>(bookings);
        }
    }
}

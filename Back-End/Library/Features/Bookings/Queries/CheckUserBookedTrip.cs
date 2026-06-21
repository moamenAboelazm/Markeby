using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Bookings.Queries
{
    public class CheckUserBookedTripQuery : IRequest<bool>
    {
        public string UserId { get; set; }
        public Guid TripId { get; set; }
    }

    public class CheckUserBookedTripQueryHandler : IRequestHandler<CheckUserBookedTripQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckUserBookedTripQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CheckUserBookedTripQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Bookings.HasUserBookedTripAsync(data.UserId, data.TripId);
        }
    }
}

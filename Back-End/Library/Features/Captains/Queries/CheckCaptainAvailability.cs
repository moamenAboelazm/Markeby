using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Captains.Queries
{
    public class CheckCaptainAvailabilityQuery : IRequest<bool>
    {
        public Guid CaptainId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class CheckCaptainAvailabilityQueryHandler : IRequestHandler<CheckCaptainAvailabilityQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckCaptainAvailabilityQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CheckCaptainAvailabilityQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Captains.IsCaptainAvailableAsync(
                request.CaptainId,
                request.StartTime,
                request.EndTime
            );
        }
    }
}

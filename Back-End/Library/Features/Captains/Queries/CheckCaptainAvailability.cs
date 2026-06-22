using Library.IRepository;
using MediatR;

namespace Library.Features.Captains.Queries
{
    public class CheckCaptainAvailabilityQuery : IRequest<bool>
    {
        public Guid CaptainId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class CheckCaptainAvailabilityQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<CheckCaptainAvailabilityQuery, bool>
    {
        public async Task<bool> Handle(CheckCaptainAvailabilityQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Captains.IsCaptainAvailableAsync(data.CaptainId, data.StartTime, data.EndTime);
        }
    }
}